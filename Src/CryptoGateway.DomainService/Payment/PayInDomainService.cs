using Bat.Core;
using CryptoGateway.Domain.Entities.User;
using Microsoft.Extensions.Configuration;
using CryptoGateway.Domain.Entities.PayIn;
using CryptoGateway.Domain.Entities.UserCredit;
using CryptoGateway.Domain.Entities.CustodyAccount;
using CryptoGateway.Domain.Entities.PayIn.ValueObjects;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Transaction.ValueObjects;

namespace CryptoGateway.DomainService.Payment;

public class PayInDomainService : IPayInDomainService
{
    private readonly IConfiguration _configuration;
    private readonly IPayInRepository _payInRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICustodyAccRepository _custodyAccRepository;
    public PayInDomainService(IConfiguration configuration, 
        IPayInRepository payInRepository, IUserRepository userRepository, ICustodyAccRepository custodyAccRepository)
    {
        _configuration = configuration;
        _payInRepository = payInRepository;
        _userRepository = userRepository;
        _custodyAccRepository = custodyAccRepository;
    }

    public async Task ApplyPayInTransaction(PayInExternalId payInExternalId, CustodyAccountId custodyAccountId, int userId, 
        CustomerId customerId, Money value, TxId txId, IBlockedCredit blockedCredit)
    {
        var user = await _userRepository.Load(userId);
        if (user == null)
        {
            FileLoger.CriticalInfo($"User id {userId} is invalid");
            return;
        }

        var custodyAccount = await _custodyAccRepository.Load(custodyAccountId);
        if (custodyAccount == null)
        {
            FileLoger.CriticalInfo($"Custody Account id {custodyAccountId} is invalid");
            return;
        }

        var commissionPercentage = _configuration["CustomSettings:Commission:PayIn:Percentage"]!;
        var commissionFixedValue = _configuration["CustomSettings:Commission:PayIn:FixedValue"]!;

        var payIn = PayIn.Create(payInExternalId, userId, customerId, value, txId);
        payIn.SetCommission(value, Convert.ToDecimal(commissionPercentage), Convert.ToDecimal(commissionFixedValue));
        payIn.AssignCustodyAccount(custodyAccount);
        payIn.CustodyAccount.ContractAccount.ActivateContractAcc();
        _payInRepository.Add(payIn);

        user.GetUserCredit(value.CurrencyType)
            .ChargeCredit(TransActionType.PayIn,
                value.Amount,
                Convert.ToDecimal(commissionPercentage),
                Convert.ToDecimal(commissionFixedValue),
                blockedCredit);
    }
}