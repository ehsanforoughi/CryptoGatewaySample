using Dapper;
using Bat.Core;
using System.Data.Common;
using CryptoGateway.Service.Resources;
using CryptoGateway.Service.Models.User;

namespace CryptoGateway.Service.Contracts.Query;

public static class UserQueries
{
    public static async Task<IResponse<object>> Query(
        this DbConnection connection, UserQueryModels.GetUserCredits query)
    {
        var result = await connection.QueryAsync<UserReadModels.UserListItem>(
            @"SELECT uc.[UserCreditId] AS [Id]
                ,u.[UserExternalId] AS [UserId]
                ,uc.[Value] AS RealBalance
	            ,(SELECT uc.[Value] - ISNULL(SUM(ISNULL([Value], 0)), 0) 
                  FROM [Payment].[Payout] po 
                  WHERE po.UserId = uc.UserId AND 
                        po.[CurrencyType] = uc.CurrencyType AND 
                        po.[State] = 1) AS AvailableBalance
                ,uc.[CurrencyType] AS [CType]
  	            ,FORMAT(uc.[CreatedAt], 'yyyy-MM-dd hh:mm tt') AS CreatedAt
 	            ,FORMAT(uc.[ModifiedAt], 'yyyy-MM-dd hh:mm tt') AS ModifiedAt
             FROM [Base].[UserCredit] uc INNER JOIN
	              [Base].[User] u ON uc.[UserId] = u.[UserId]
             WHERE uc.[IsDeleted] = 0 AND u.[UserExternalId] = @UserId",
            new
            {
                query.UserId
            });
        return Response<object>.Success(result, ServiceMessages.Success);
    }
}