using Dapper;
using Bat.Core;
using System.Data.Common;
using CryptoGateway.Service.Models;
using CryptoGateway.Service.Handlers;
using CryptoGateway.Service.Resources;
using CryptoGateway.Service.Models.Payout;

namespace CryptoGateway.Service.Contracts.Query;

public static class PayoutQueries
{
    public static async Task<IResponse<object>> Query(
        this DbConnection connection, PayoutQueryModels.GetFiatPayouts query)
    {
        var result = await PaginationQueryHandler<PayoutReadModels.FiatPayoutListItem>
            .CreateInstance(connection,
            selectClause:
                @"SELECT [PayoutId]
                      ,u.[UserExternalId] AS [UserId]
                      ,u.[FirstName] + ' ' + u.[LastName] AS [UserFullName]
                      ,ua.[FirstName] + ' ' + ua.[LastName] AS [ApproverFullName]
                      ,[Value]
                      ,[CurrencyType] AS [CType]
                      ,ba.[Type] AS [BType]
                      ,ba.[CardNumber]
                      ,p.[State]
                      ,[BankTrackingCode]
                      ,p.[Desc] AS [ApproverDesc]
                      ,FORMAT(p.[CreatedAt], 'yyyy-MM-dd hh:mm tt') AS CreatedAt
                      ,FORMAT(p.[ModifiedAt], 'yyyy-MM-dd hh:mm tt') AS ModifiedAt",
            fromClause:
                @"FROM [Payment].[Payout] p INNER JOIN
	                   [Base].[User] u ON p.UserId = u.UserId LEFT JOIN
	                   [Base].[User] ua ON p.ApprovedBy = ua.UserId INNER JOIN
	                   [Base].[BankAccount] ba ON p.BankAccountId = ba.BankAccountId",
            whereClause:
                @"WHERE p.[IsDeleted] = 0 AND [TransferType] = 2 AND u.[UserExternalId] = @UserId",
            pagingClause:
                @"ORDER BY p.CreatedAt DESC
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
        this DbConnection connection, PayoutQueryModels.GetCryptoPayouts query)
    {
        var result = await PaginationQueryHandler<PayoutReadModels.CryptoPayoutListItem>
            .CreateInstance(connection,
            selectClause:
                @"SELECT [PayoutId]
                      ,u.[UserExternalId] AS [UserId]
	                  ,u.[FirstName] + ' ' + u.[LastName] AS [UserFullName]
                      ,ua.[FirstName] + ' ' + ua.[LastName] AS [ApproverFullName]
                      ,[Value]
                      ,[CurrencyType] AS [CType]
                      ,w.[Address] AS [WalletAddress]
                      ,p.[State]
                      ,p.TransactionUrl
                      ,p.[Desc] AS [ApproverDesc]
                      ,FORMAT(p.[CreatedAt], 'yyyy-MM-dd hh:mm tt') AS CreatedAt
                      ,FORMAT(p.[ModifiedAt], 'yyyy-MM-dd hh:mm tt') AS ModifiedAt",
            fromClause:
                @"FROM [Payment].[Payout] p INNER JOIN
	                   [Base].[User] u ON p.UserId = u.UserId LEFT JOIN
	                   [Base].[User] ua ON p.ApprovedBy = ua.UserId INNER JOIN
	                   [Base].[Wallet] w ON p.WalletId = w.WalletId",
            whereClause:
            @"WHERE p.[IsDeleted] = 0 AND [TransferType] = 4 AND u.[UserExternalId] = @UserId",
            pagingClause:
            @"ORDER BY p.CreatedAt DESC
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
}