using Dapper;
using Bat.Core;
using System.Data.Common;
using CryptoGateway.Service.Models;
using CryptoGateway.Service.Handlers;
using CryptoGateway.Service.Resources;
using CryptoGateway.Service.Models.PayIn;

namespace CryptoGateway.Service.Contracts.Query;

public static class PayInQueries
{
   public static async Task<IResponse<object>> Query(
        this DbConnection connection, PayInQueryModels.GetPayIns query)
    {
        var result = await PaginationQueryHandler<PayInReadModels.PayInListItem>.CreateInstance(connection,
            selectClause:
            @"SELECT [PayInExternalId] AS [PayInId]
                    ,u.[UserExternalId] AS [UserId]
                    ,u.FirstName + ' ' + u.LastName AS [UserFullName]
                    ,[CustomerId]
                    ,[Value]
                    ,[CurrencyType] AS [VCType]
	                ,([Value] * ([ComPercentage] * 0.01)) + [ComFixedValue] AS [CommissionValue]
                    ,[ComCurrencyType] AS [CCType]
                    ,[TxId]
                    ,[CustomerContact] = (SELECT TOP 1 [CustomerContact] FROM [Payment].[Payment] p WHERE CustomerId = payin.CustomerId ORDER BY p.CreatedAt DESC )
	                ,FORMAT(payin.[CreatedAt], 'yyyy-MM-dd hh:mm tt') AS CreatedAt",
            fromClause:
            @"FROM [Payment].[PayIn] payin INNER JOIN
	               [Base].[User] u ON payin.UserId = u.UserId",
            whereClause:
            @"WHERE payin.[IsDeleted] = 0 AND u.[UserExternalId] = @UserId",
            pagingClause:
            @"ORDER BY payin.CreatedAt DESC
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