using Dapper;
using Bat.Core;
using System.Data.Common;
using CryptoGateway.Service.Resources;
using CryptoGateway.Service.Models.Wallet;
using CryptoGateway.Service.Handlers;
using CryptoGateway.Service.Models;

namespace CryptoGateway.Service.Contracts.Query;

public static class WalletQueries
{
    public static async Task<IResponse<object>> Query(
        this DbConnection connection, WalletQueryModels.GetWallets query)
    {
        var result = await PaginationQueryHandler<WalletReadModels.WalletListItem>.CreateInstance(connection,
            selectClause:
                @"SELECT [WalletId]
                   ,[Title]
                   ,u1.[UserExternalId] AS [UserId]
                   ,[ApprovedBy]
	               ,u2.FirstName + ' ' + u2.LastName AS [Approver]
                   ,[State]
                   ,w.[CurrencyId]
	               ,c.[Type] AS [CType]
                   ,w.[Network]
                   ,[Address]
                   ,[Memo]
                   ,[Tag]
                   ,[Desc]
                   ,FORMAT(w.[CreatedAt], 'yyyy-MM-dd hh:mm tt') AS CreatedAt
                   ,FORMAT(w.[ModifiedAt], 'yyyy-MM-dd hh:mm tt') AS ModifiedAt",
            fromClause:
                @"FROM [Base].[Wallet] w INNER JOIN
	              [Base].[User] u1 ON w.UserId = u1.UserId LEFT JOIN
	              [Base].[User] u2 ON w.[ApprovedBy] = u2.UserId INNER JOIN
	              [Base].[Currency] c ON w.CurrencyId = c.CurrencyId",
            whereClause:
                @"WHERE w.[IsDeleted] = 0 AND u1.UserExternalId = @UserId",
            pagingClause:
                @"ORDER BY w.CreatedAt DESC
                  OFFSET @Offset ROWS
                  FETCH NEXT @PageSize ROWS ONLY",
            pagination: new PaginationModel
            {
                Page = query.Page,
                PageSize = query.PageSize
            },
            parameters: new DynamicParameters(new { UserId = query.UserId })
        ).Handle();

        return Response<object>.Success(result, ServiceMessages.Success);
    }

    public static async Task<IResponse<object>> Query(
        this DbConnection connection, WalletQueryModels.GetApprovedList query)
    {
        var res = await connection.QueryAsync<WalletReadModels.ApprovedListItem>(
            @"SELECT [WalletId]
                  ,[Title] AS WalletTitle
                  ,[Address] AS WalletAddress
                  ,c.Code AS [CurrencyType]
             FROM [Base].[Wallet] w INNER JOIN 
	              [Base].[User] u ON w.UserId = u.UserId INNER JOIN
	              [Base].[Currency] c ON w.CurrencyId = c.CurrencyId
             WHERE w.[IsDeleted] = 0 AND w.[State] = 2 AND u.[UserExternalId] = @UserId"
            ,
            new
            {
                query.UserId
            });

        return Response<object>.Success(new { Items = res.ToList() }, ServiceMessages.Success);
    }
}