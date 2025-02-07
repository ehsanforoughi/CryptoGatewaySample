using Quartz;
using Bat.Core;
using System.Numerics;
using System.Data.Common;
using CryptoGateway.Framework;
using CryptoGateway.Service.Resources;
using CryptoGateway.Service.Models.Payment;
using CryptoGateway.Service.Contracts.Query;
using CryptoGateway.Domain.Entities.Payment;
using CryptoGateway.Domain.Entities.UserCredit;
using Microsoft.Extensions.DependencyInjection;
using CryptoGateway.Domain.Entities.Notification;
using CryptoGateway.Service.Models.ContractAccount;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Payment.ValueObjects;
using CryptoGateway.Service.Strategies.Notification.Creation;
using CryptoGateway.Domain.Entities.Notification.ValueObjects;


namespace CryptoGateway.Service.Quartz.Jobs.Payment;

[DisallowConcurrentExecution]
public class InquiryPaymentStatusJob : IJob, IDisposable
{
    private readonly IServiceProvider _serviceProvider;

    public InquiryPaymentStatusJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            FileLoger.Message($"InquiryPaymentStatusJob is started: {DateTime.Now.ToString("HH:mm:ss")}");
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            var connection = scope.ServiceProvider.GetService<DbConnection>();
            var request = new PaymentQueryModels.GetWaitingPayments();
            var waitingPayments = await connection.Query(request);
            var paymentDomainService = scope.ServiceProvider.GetService<IPaymentDomainService>();
            var blockedCredit = scope.ServiceProvider.GetService<IBlockedCredit>();
            var notificationRepository = scope.ServiceProvider.GetService<INotificationRepository>();


            foreach (var payment in waitingPayments.ToList())
            {
                var contractTransactionRequest = new ContractAccountQueryModels.GetContractTransactionList
                {
                    ContractAccountId = payment.ContractAccountId
                };
                var contractTransactions = await connection!.Query(contractTransactionRequest);

                foreach (var transaction in contractTransactions)
                {
                    var timeStamp = ConvertTimestampToDateTime(transaction.TimeStamp);
                    if (timeStamp > payment.CreatedAt && transaction.ContractType.Equals("Transfer"))
                    {
                        //if (string.Equals(((CurrencyType)payment.PayCurrencyType).ToString(), transaction.Symbol, StringComparison.CurrentCultureIgnoreCase))
                        //{
                        //    if (payment.PayAmount != transaction.Amount || payment.AddressBase58 != transaction.ToAddress) continue;

                        //    await paymentDomainService.ApplyPaymentTransaction(new PaymentId(payment.PaymentId),
                        //        Money.FromDecimal(transaction.Amount, CurrencyType.USDT), blockedCredit);

                        //    SendNotificationToUser(notificationRepository, transaction.Amount, payment.CustomerId,
                        //        payment.UserId, payment.UserEmail, payment.UserMobileNumber, "تتر");

                        //    await unitOfWork.Commit();
                        //}
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

    private void SendNotificationToUser(INotificationRepository repository, decimal amount, string customerId, int userId, string userEmail, string userMobileNumber, string currencyTypeStr)
    {
        var userMessage = ServiceMessages.UserPaymentIsDoneMessage
            .Fill(customerId, amount.ToString(), currencyTypeStr);

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