using Bat.Core;
using KsuidDotNet;
using CryptoGateway.Framework;
using CryptoGateway.Service.Resources;
using CryptoGateway.Domain.Extensions;
using Microsoft.Extensions.Configuration;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.Domain.Entities.Shared;
using CryptoGateway.Domain.Entities.Payment;
using CryptoGateway.Service.Contracts.Command;
using CryptoGateway.Domain.Entities.CustodyAccount;
using CryptoGateway.Domain.Entities.User.ValueObjects;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Payment.ValueObjects;
using CryptoGateway.Domain.Entities.CustodyAccount.ValueObjects;

namespace CryptoGateway.Service.Implements;

public class PaymentApplicationService : IApplicationService
{
    private readonly IConfiguration _configuration;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISpotPriceProvider _spotPriceProvider;
    private readonly IContractAccDomainService _contractAccDomainService;
    private readonly ICustodyAccRepository _custodyAccRepository;

    public PaymentApplicationService(IConfiguration configuration, IPaymentRepository paymentRepository, 
        IUserRepository userRepository, IUnitOfWork unitOfWork, ISpotPriceProvider spotPriceProvider, 
        IContractAccDomainService contractAccDomainService, ICustodyAccRepository custodyAccRepository)
    {
        _configuration = configuration;
        _paymentRepository = paymentRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _spotPriceProvider = spotPriceProvider;
        _contractAccDomainService = contractAccDomainService;
        _custodyAccRepository = custodyAccRepository;
    }

    private async Task<IResponse<object>> HandleCreatePayment(string currentUserId, PaymentCommands.V1.CreatePayment cmd)
    {
        try
        {
            var user = await _userRepository.Load(new UserExternalId(currentUserId));
            if (user == null)
                return Response<object>.Error(ServiceMessages.EntityIdNotFound.Fill(currentUserId));

            var customerId = CustomerId.FromString(cmd.CustomerId);
            var orderId = OrderId.FromString(cmd.OrderId);

            var currencyType = cmd.PayCurrencyCode.ParseCurrencyType();
            if (currencyType != CurrencyType.USDT)
                return Response<object>.Error(ServiceMessages.OnlyTetherIsAllowed);

            if (await _paymentRepository.Exists(user.Id, customerId, orderId, currencyType))
                return Response<object>.Error(ServiceMessages.EntityIsExistsByParameters
                    .Fill("Payment"));

            #region Create Custody Account

            var custodyAccount = await _custodyAccRepository.Load(user.Id, customerId);
            if (custodyAccount is null)
            {
                custodyAccount = CustodyAccount.Create(new CustodyAccExternalId(Ksuid.NewKsuid()),
                currencyType, CustodyAccountTitle.FromString("PaymentLink"), user.Id, customerId);

                var contractAccount =
                    await _contractAccDomainService.GetLatestContractAccount(user.Id, customerId);
                custodyAccount.AssignContractAccount(contractAccount);
            }
            #endregion

            #region Create Payment And Assign Custody Account to it
            var payment =
                Payment.CreatePayment(new PaymentExternalId(Ksuid.NewKsuid()), user.Id,
                    Money.FromDecimal(cmd.PriceAmount, CurrencyType.IRR), Money.FromNotClearAmount(currencyType),
                    customerId, CustomerContact.FromString(cmd.CustomerContact), orderId, OrderDescription.FromString(cmd.OrderDescription),
                    _spotPriceProvider);

            payment.AssignCustodyAccount(custodyAccount);
            #endregion

            _paymentRepository.Add(payment);
            await _unitOfWork.Commit();

            var baseUrl = $"{_configuration["CustomSettings:ApplicationBaseUrl"]}/link/payment";
            var result = new
            {
                PaymentId = payment.PaymentExternalId.Value,
                CustomerId = payment.CustomerId.Value,
                OrderId = payment.OrderId.Value,
                OrderDescription = payment.OrderDescription.Value,
                Price_Amount = payment.Price?.Amount ?? 0,
                Price_Currency = payment.Price.CurrencyType.ToString(),
                Pay_Amount = payment.Pay?.Amount ?? 0,
                Pay_Currency = payment.Pay.CurrencyType.ToString(),
                PayAddress = custodyAccount.ContractAccount.Address.Base58Value,
                Network = "TRC20",
                paymentLink = $"{baseUrl}/{payment.PaymentExternalId.Value}",
                Created_At = payment.InsertDateMi.ToString("yyyy-MM-dd HH:mm:ss"),
                //Updated_At = payment.ModifyDateMi,
            };

            return Response<object>.Success(result, ServiceMessages.Success);
        }
        catch (Exception e)
        {
            FileLoger.Error(e);
            return Response<object>.Error(e.Message);
        }
    }
    
    public Task<IResponse<object>> Handle(string currentUserId, object command) =>
        command switch
        {
            PaymentCommands.V1.CreatePayment cmd => HandleCreatePayment(currentUserId, cmd),
            //PaymentCommands.V1.UpdateEstimatedPayAmount cmd => HandleUpdateEstimatedPayAmount(cmd.PaymentId),
            _ => throw new ArgumentOutOfRangeException(nameof(command), command, null)
        };
}