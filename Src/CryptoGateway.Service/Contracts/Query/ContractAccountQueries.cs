using Dapper;
using System.Data.Common;
using CryptoGateway.Service.Models.ContractAccount;

namespace CryptoGateway.Service.Contracts.Query;

public static class ContractAccountQueries
{
    public static Task<IEnumerable<ContractAccReadModels.ContractAccItems>> Query(
        this DbConnection connection, ContractAccountQueryModels.GetContractAccountList query)
        => connection.QueryAsync<ContractAccReadModels.ContractAccItems>(
            @"SELECT [ContractAccountId], [AddressBase58], [AddressHex]
             FROM [Contract].[ContractAccount]
             WHERE [IsDeleted] = 0",
            new { });


    public static Task<IEnumerable<ContractAccReadModels.ContractTransactionItems>> Query(
        this DbConnection connection, ContractAccountQueryModels.GetContractTransactionList query)
        => connection.QueryAsync<ContractAccReadModels.ContractTransactionItems>(
            @"SELECT [ContractAccountId], [TxId], [ContractType], [Amount], [ContractAddress], 
                     [ContractData], [CreatedAt], [TimeStamp], [Symbol], [ToAddress]
              FROM [Contract].[ContractTransaction]
              WHERE ContractAccountId = @ContractAccountId AND (ContractType = @ContractType OR ContractType = null)",
            new
            {
                query.ContractType,
                query.ContractAccountId
            });

    public static Task<IEnumerable<ContractAccReadModels.ContractTransactionItems>> Query(
        this DbConnection connection, ContractAccountQueryModels.GetNewContractTransactionList query)
        => connection.QueryAsync<ContractAccReadModels.ContractTransactionItems>(
            @"SELECT [ContractAccountId], [TxId], [ContractType], [Amount], [ContractAddress], 
                     [ContractData], [CreatedAt], [TimeStamp], [Symbol], [ToAddress]
              FROM [Contract].[ContractTransaction]
              WHERE [TxId] NOT IN (select [TxId] FROM [Payment].[PayIn]) AND 
                    ContractAccountId = @ContractAccountId AND (ContractType = @ContractType OR ContractType = null)",
            new
            {
                query.ContractType,
                query.ContractAccountId
            });
}