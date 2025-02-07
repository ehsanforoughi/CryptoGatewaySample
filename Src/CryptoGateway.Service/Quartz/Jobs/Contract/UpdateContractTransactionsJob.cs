using Quartz;
using Bat.Core;
using System.Data.Common;
using CryptoGateway.Framework;
using CryptoGateway.Service.Contracts.Query;
using Microsoft.Extensions.DependencyInjection;
using CryptoGateway.Service.Models.ContractAccount;
using CryptoGateway.Domain.Entities.ContractAccount;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.DomainService.ExternalWebServices.NodeJsApi;
using CryptoGateway.Domain.Entities.ContractAccount.ValueObjects;

namespace CryptoGateway.Service.Quartz.Jobs.Contract;

[DisallowConcurrentExecution]
public class UpdateContractTransactionsJob : IJob, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    public UpdateContractTransactionsJob(IServiceProvider serviceProvider)
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
            var tronWeb = scope.ServiceProvider.GetService<ITronWeb>();
            var contractRequest = new ContractAccountQueryModels.GetContractAccountList();
            var contracts = await connection!.Query(contractRequest);
            var contractAccountRepository = scope.ServiceProvider.GetService<IContractAccountRepository>();

            foreach (var contract in contracts)
            {
                var contractTransactionRequest = new ContractAccountQueryModels.GetContractTransactionList
                {
                    ContractAccountId = contract.ContractAccountId,
                    ContractType = null
                };
                var contractTransactions = await connection!.Query(contractTransactionRequest);

                await ApplyAllTransactions(tronWeb, contract, contractTransactions, contractAccountRepository);
                await ApplyTrc20Transactions(tronWeb, contract, contractTransactions, contractAccountRepository);

                if (await unitOfWork.HasChanges())
                    await unitOfWork!.Commit();
            }
        }
        catch (Exception e)
        {
            FileLoger.CriticalError(e);
        }
    }

    private static async Task ApplyAllTransactions(ITronWeb? tronWeb, ContractAccReadModels.ContractAccItems contract,
        IEnumerable<ContractAccReadModels.ContractTransactionItems> contractTransactions, IContractAccountRepository? contractAccountRepository)
    {
        var tronTransactions = await tronWeb!.GetTransactionsAsync(contract.AddressBase58);
        if (tronTransactions == null || tronTransactions.result.data.Length == 0) return;

        for (int i = 0; i < tronTransactions.result.data.Length; i++)
        {
            var transaction = tronTransactions.result.data[i];
            var txId = transaction.txID != null ? transaction.txID : transaction.tx_id;
            //if (contractTransactions.ToList().Any(x => x.TxId.Trim() == txId.Trim()))
            if (await contractAccountRepository?.Exists(TxId.FromString(txId))!)
                continue;

            var coa = await contractAccountRepository!.Load(new ContractAccountId(contract.ContractAccountId));
            if (coa is null)
                continue;

            coa.AddContractTransaction(
                txId: transaction.txID is not null ? transaction.txID : transaction.tx_id,
                timestamp: transaction.raw_data is not null ? transaction.raw_data.timestamp : transaction.block_timestamp,
                contractType: transaction.raw_data is not null
                    ? transaction.raw_data?.contract.FirstOrDefault()?.type
                    : transaction.data.note,
                contractResource: transaction.raw_data?.contract.FirstOrDefault()?.parameter.value.resource,
                contractData: transaction.raw_data?.contract.FirstOrDefault()?.parameter?.value?.data,
                contractAddress: transaction.raw_data?.contract.FirstOrDefault()?.parameter?.value?.contract_address,
                ownerAddress: transaction.raw_data?.contract.FirstOrDefault()?.parameter?.value?.owner_address,
                receiverAddress: transaction.raw_data?.contract.FirstOrDefault()?.parameter?.value?.receiver_address,
                fromAddress: transaction.from_address,
                toAddress: transaction.raw_data is not null
                    ? transaction.raw_data?.contract.FirstOrDefault()?.parameter?.value?.to_address
                    : transaction.to_address,
                amount: transaction.raw_data is not null
                    ? (decimal)transaction.raw_data?.contract.FirstOrDefault()?.parameter?.value?.amount
                    : (decimal)transaction?.data?.call_value?._,
                expiration: transaction.raw_data?.expiration,
                refBlockBytes: transaction.raw_data?.ref_block_bytes,
                refBlockHash: transaction.raw_data?.ref_block_hash,
                feeLimit: transaction.raw_data?.fee_limit,
                signature: transaction.signature?.FirstOrDefault()!,
                energyUsage: transaction.energy_usage,
                energyFee: transaction.energy_fee,
                gasLimit: null,
                gasPrice: null,
                bandwidthUsage: null,
                bandwidthFee: null);
        }
    }

    private static async Task ApplyTrc20Transactions(ITronWeb? tronWeb, ContractAccReadModels.ContractAccItems contract,
    IEnumerable<ContractAccReadModels.ContractTransactionItems> contractTransactions, IContractAccountRepository? contractAccountRepository)
    {
        var tronTransactions = await tronWeb!.GetTrc20TransactionsAsync(contract.AddressBase58);
        if (tronTransactions == null || tronTransactions.result.data.Length == 0) return;

        for (int i = 0; i < tronTransactions.result.data.Length; i++)
        {
            var transaction = tronTransactions.result.data[i];
            var txId = transaction.transaction_id;
            //if (contractTransactions.ToList().Any(x => x.TxId.Trim() == txId.Trim() && x.ContractType.ToLower() == "transfer"))
            var hasTxId = await contractAccountRepository?.Exists(TxId.FromString(txId))!;
            FileLoger.Info($"TxId = {txId}");
            FileLoger.Info($"hasTxId = {hasTxId}");
            if (hasTxId)
                continue;

            var coa = await contractAccountRepository!.Load(new ContractAccountId(contract.ContractAccountId));
            if (coa is null)
                continue;

            coa.AddContractTrc20Transaction(
                txId: txId,
                timestamp: transaction.block_timestamp,
                contractType: transaction.type,
                ownerAddress: transaction.token_info.address,
                receiverAddress: transaction.from,
                toAddress: transaction.to,
                amount: ConvertToDecimal(transaction.value, transaction.token_info.decimals),
                symbol: transaction.token_info.symbol
                );
        }
    }

    private static decimal ConvertToDecimal(long number, int decimalPlaces)
    {
        decimal divisor = (decimal)Math.Pow(10, decimalPlaces);
        return number / divisor;
    }

    public void Dispose() => GC.SuppressFinalize(this);
}