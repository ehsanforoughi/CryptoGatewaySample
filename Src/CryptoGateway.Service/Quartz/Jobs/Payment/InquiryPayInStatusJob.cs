using Quartz;
using Bat.Core;
using KsuidDotNet;
using System.Numerics;
using System.Data.Common;
using CryptoGateway.Framework;
using CryptoGateway.Service.Resources;
using CryptoGateway.Domain.Entities.PayIn;
using CryptoGateway.Service.Contracts.Query;
using Microsoft.Extensions.DependencyInjection;
using CryptoGateway.Domain.Entities.UserCredit;
using CryptoGateway.Domain.Entities.Notification;
using CryptoGateway.Service.Models.CustodyAccount;
using CryptoGateway.Service.Models.ContractAccount;
using CryptoGateway.Domain.Entities.PayIn.ValueObjects;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Service.Strategies.Notification.Creation;
using CryptoGateway.Domain.Entities.Notification.ValueObjects;
using CryptoGateway.DomainService.ExternalWebServices.SunCentre;

namespace CryptoGateway.Service.Quartz.Jobs.PayIn;

[DisallowConcurrentExecution]
public class InquiryPayInStatusJob : IJob, IDisposable
{
    private readonly IServiceProvider _serviceProvider;

    public InquiryPayInStatusJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            var connection = scope.ServiceProvider.GetService<DbConnection>();
            var custodyAccountsRequest = new CustodyAccQueryModels.GetAllCustodyAccounts();
            var custodyAccounts = await connection.Query(custodyAccountsRequest);
            var payInDomainService = scope.ServiceProvider.GetService<IPayInDomainService>();
            var blockedCredit = scope.ServiceProvider.GetService<IBlockedCredit>();
            var notificationRepository = scope.ServiceProvider.GetService<INotificationRepository>();
            var sunCentreECommerceApi = scope.ServiceProvider.GetService<ISunCentreECommerceApi>();
            //var usdtSmartContract = "41a614f803b6fd780986a42c78ec9c7f77e6ded13c";

            foreach (var custodyAccount in custodyAccounts)
            {
                var newContractTransactionRequest = new ContractAccountQueryModels.GetNewContractTransactionList
                {
                    ContractAccountId = custodyAccount.ContractAccountId,
                    ContractType = "Transfer"
                };
                var newContractTransactions = await connection!.Query(newContractTransactionRequest);

                foreach (var newTransaction in newContractTransactions)
                {
                    var timeStamp = ConvertTimestampToDateTime(newTransaction.TimeStamp);
                    if (timeStamp >= DateTime.Now.AddDays(-10) && newTransaction.ContractType.Equals("Transfer"))
                    {
                        await payInDomainService.ApplyPayInTransaction(new PayInExternalId(Ksuid.NewKsuid()),
                            new CustodyAccountId(custodyAccount.CustodyAccountId), custodyAccount.UserId,
                            CustomerId.FromString(custodyAccount.CustomerId),
                            Money.FromDecimal(newTransaction.Amount, CurrencyType.USDT),
                            TxId.FromString(newTransaction.TxId), blockedCredit);

                        await sunCentreECommerceApi.UpdateUserWallet(Convert.ToInt32(custodyAccount.CustomerId),
                            newTransaction.Amount, "credit", "", 0);

                        SendNotificationToBeneficiaries(notificationRepository, newTransaction.Amount, custodyAccount.CustomerId, custodyAccount.CustomerContact,
                            custodyAccount.UserId, custodyAccount.UserEmail, custodyAccount.UserMobileNumber, "تتر");

                        await unitOfWork.Commit();
                    }
                }
            }
        }
        catch (Exception e)
        {
            FileLoger.CriticalError(e);
        }
    }

    private static DateTime ConvertTimestampToDateTime(long timestamp)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
        return dateTimeOffset.LocalDateTime;
    }

    private static decimal FromSun(decimal amount) => amount / 1000000;

    private static string ExtractMethodName(string dataHex)
    {
        string methodId = dataHex.Substring(0, 8);
        return methodId;
    }
    private static long ExtractAmountFromDataHex(string dataHex)
    {
        var amountHex = dataHex.Substring(72);
        var amountDecimal = Convert.ToInt64(amountHex, 16);
        return amountDecimal;
    }

    private static string ExtractRecipientAddressFromDataHex(string dataHex)
    {
        string addressHex = dataHex.Substring(8, 64);
        string recipientAddress = BigInteger.Parse(addressHex, System.Globalization.NumberStyles.HexNumber).ToString("x");
        return recipientAddress;
    }

    private void SendNotificationToBeneficiaries(INotificationRepository repository, decimal amount, string customerId, string customerContact, 
        int userId, string userEmail, string userMobileNumber, string currencyTypeStr)
    {
        #region Send Notification To Customer
        NotificationType? notificationType = null;
        if (customerContact.IsEmail())
            notificationType = NotificationType.Email;
        else if (long.TryParse(customerContact, out _))
            notificationType = NotificationType.Sms;

        if (notificationType != null)
        {
            var customerMessage = ServiceMessages.CustomerDepositIsDoneMessage
                .Fill(FromSun(amount).ToString(), currencyTypeStr);
            var customerNotification = NotificationCreationFactory.GetStrategy((NotificationType)notificationType)
                .Creation(userId, customerContact, NotificationActionType.FreeTemplate, customerMessage);
            repository.Add(customerNotification);
        }
        #endregion

        var userMessage = ServiceMessages.UserDepositIsDoneMessage
            .Fill(customerId, FromSun(amount).ToString(), currencyTypeStr);+

        #region Send Notification To User (Email and MobileNumber if available)
        if (!string.IsNullOrWhiteSpace(userEmail))
        {
            var userNotification = NotificationCreationFactory.GetStrategy(NotificationType.Email)
                .Creation(userId, userEmail, NotificationActionType.FreeTemplate,
                    userMessage);
            repository.Add(userNotification);
        }

        if (!string.IsNullOrWhiteSpace(userMobileNumber))
        {
            var userNotification = NotificationCreationFactory.GetStrategy(NotificationType.Sms)
                .Creation(userId, $"0{userMobileNumber}",
                    NotificationActionType.FreeTemplate, userMessage);
            repository.Add(userNotification);
        }
        #endregion
    }

    public void Dispose() => GC.SuppressFinalize(this);
}