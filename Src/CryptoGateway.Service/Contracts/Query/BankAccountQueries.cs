using Dapper;
using Bat.Core;
using System.Data.Common;
using CryptoGateway.Service.Models;
using CryptoGateway.Service.Handlers;
using CryptoGateway.Service.Resources;
using CryptoGateway.Service.Models.BankAccount;
using CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

namespace CryptoGateway.Service.Contracts.Query;

public static class BankAccountQueries
{
    public static async Task<IResponse<object>> Query(
        this DbConnection connection, BankAccountQueryModels.GetBankAccounts query)
    {
        var result = await PaginationQueryHandler<BankAccountReadModels.BankAccountListItem>.CreateInstance(connection,
            selectClause:
            @"SELECT ba.[BankAccountId]
                   ,u1.[UserExternalId] AS [UserId]
                   ,ba.[Title]
                   ,u2.FirstName + ' ' + u2.LastName AS [ApprovedBy]
                   ,ba.[State]
                   ,ba.[Type] AS [BankType]
                   ,ba.[CardNumber]
                   ,ba.[Sheba]
                   ,ba.[AccountNumber]
                   ,ba.[Desc]
                   ,FORMAT(ba.[CreatedAt], 'yyyy-MM-dd hh:mm') AS [CreatedAt]
                   ,FORMAT(ba.[ModifiedAt], 'yyyy-MM-dd hh:mm') AS [ModifiedAt]",
            fromClause:
            @"FROM [Base].[BankAccount] ba INNER JOIN
	                [Base].[User] u1 ON ba.UserId = u1.UserId LEFT JOIN
	                [Base].[User] u2 ON ba.ApprovedBy = u2.UserId",
            whereClause:
            @"WHERE ba.[IsDeleted] = 0 AND u1.[UserExternalId] = @UserId",
            pagingClause:
            @"ORDER BY ba.CreatedAt DESC
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
        this DbConnection connection, BankAccountQueryModels.GetApprovedList query)
    {
        var res = await connection.QueryAsync<BankAccountReadModels.ApprovedListItem>(
            @"SELECT [BankAccountId]
                  ,[Type] AS [BankType]
                  ,[CardNumber]
             FROM [Base].[BankAccount] ba INNER JOIN 
	              [Base].[User] u ON ba.UserId = u.UserId
             WHERE ba.[IsDeleted] = 0 AND [State] = 2 AND u.[UserExternalId] = @UserId
             ORDER BY [Type]"
            ,
            new
            {
                query.UserId
            });

        return Response<object>.Success(new {Items = res.ToList()}, ServiceMessages.Success);
    }

    public async static Task<IResponse<object>> Query(
        this DbConnection connection,
        BankAccountQueryModels.GetBankTypes query)
    {
        var result = new List<BankAccountReadModels.BankTypeListItem>();
        EnumExtension.GetEnumElements<BankType>()
            .ForEach(element =>
            {
                if (!new List<string>() { "26", "40" }.Contains(element.Value.ToString())) // Remove ZarinPal & HillaPay as bank
                    result.Add(new BankAccountReadModels.BankTypeListItem
                    {
                        BankName = element.Description, 
                        BankType = element.Name, 
                        BankTypeId = int.Parse(element.Value.ToString())
                    });
            });

        return Response<object>.Success(result, ServiceMessages.Success);
    }
}