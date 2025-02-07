using Bat.Core;
using CryptoGateway.Domain.Entities.User;
using Microsoft.Extensions.Configuration;
using CryptoGateway.Domain.Entities.Payment;
using CryptoGateway.Domain.Entities.UserCredit;
using CryptoGateway.Domain.Entities.Payment.ValueObjects;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Transaction.ValueObjects;

namespace CryptoGateway.DomainService.Payment;

public class PaymentDomainService : IPaymentDomainService
{
    private readonly IConfiguration _configuration;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUserRepository _userRepository;
    public PaymentDomainService(IConfiguration configuration, 
        IPaymentRepository paymentRepository, IUserRepository userRepository)
    {
        _configuration = configuration;
        _paymentRepository = paymentRepository;
        _userRepository = userRepository;
    }

    //public async Task ApplyPaymentTransaction(PaymentId paymentId, Money value, IBlockedCredit blockedCredit)
    //{
    //    //var commissionPercentage = _configuration["CustomSettings:Commission:Payment:Percentage"]!;
    //    //var commissionFixedValue = _configuration["CustomSettings:Commission:Payment:FixedValue"]!;

    //    var payment = await _paymentRepository.Load(paymentId);
    //    if (payment == null)
    //    {
    //        FileLoger.CriticalInfo($"Payment with id {paymentId.Value} is invalid");
    //        return;
    //    }

    //    //payment.ApprovePayment(rawValue,
    //    //    Convert.ToDecimal(commissionPercentage),
    //    //    Convert.ToDecimal(commissionFixedValue));

    //    var user = await _userRepository.Load(payment.UserId);
    //    if (user == null)
    //    {
    //        FileLoger.CriticalInfo($"User id {payment.UserId} is invalid");
    //        return;
    //    }

    //    payment.CustodyAccount.ContractAccount.ActivateContractAcc();

    //    user.GetUserCredit(value.CurrencyType)
    //        .ChargeCredit(TransActionType.Payment,
    //            value.Amount,
    //            Convert.ToDecimal(commissionPercentage),
    //            Convert.ToDecimal(commissionFixedValue),
    //            blockedCredit);
    //}
}