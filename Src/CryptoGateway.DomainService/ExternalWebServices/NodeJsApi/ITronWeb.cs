using CryptoGateway.DomainService.ExternalWebServices.NodeJsApi.Dto;

namespace CryptoGateway.DomainService.ExternalWebServices.NodeJsApi;

public interface ITronWeb
{
    Task<CreateWalletResultDto> CreateAccountAsync();
    Task<object> GetAccountBalanceAsync(string walletAddressBase58);
    Task<GetTransactionsResultDto?> GetTransactionsAsync(string walletAddressBase58);
    Task<GetTransactionByTxIdResultDto?> GetTransactionsByTxIdAsync(string txId);
    Task<GetTransactionsTrc20ResultDto?> GetTrc20TransactionsAsync(string walletAddressBase58);
    Task<object> SendTetherAsync();
    Task<object> SendTronAsync();
}