using CryptoGateway.Domain.Entities.Payment;
using CryptoGateway.Domain.Entities.CustodyAccount;
using CryptoGateway.Domain.Entities.ContractAccount;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.DomainService.ExternalWebServices.NodeJsApi;
using CryptoGateway.Domain.Entities.ContractAccount.ValueObjects;

namespace CryptoGateway.DomainService.Payment;

public class ContractAccountDomainService : IContractAccDomainService
{
    private readonly ITronWeb _tronWeb;
    private readonly ICustodyAccRepository _custodyAccountRepository;
    private readonly IContractAccountRepository _contractAccountRepository;

    public ContractAccountDomainService(ITronWeb tronWeb, ICustodyAccRepository custodyAccountRepository, IContractAccountRepository contractAccountRepository)
    {
        _tronWeb = tronWeb;
        _custodyAccountRepository = custodyAccountRepository;
        _contractAccountRepository = contractAccountRepository;
    }

    public async Task<ContractAccount> GetLatestContractAccount(int userId, CustomerId customerId)
    {
        var lastCustodyAccount = await _custodyAccountRepository.Load(userId, customerId);
        if (lastCustodyAccount != null)
            return lastCustodyAccount.ContractAccount;

        var tronWebResult = await _tronWeb.CreateAccountAsync() ??
                            throw new SystemException("Something is wrong!");

        var account = ContractAccount
            .Create(ContractAddress.FromString(tronWebResult.Address.Base58.Trim(), tronWebResult.Address.Hex.Trim()),
                PublicKey.FromString(tronWebResult.PublicKey), PrivateKey.FromString(tronWebResult.PrivateKey));

        _contractAccountRepository.Add(account);

        return account;
    }
}