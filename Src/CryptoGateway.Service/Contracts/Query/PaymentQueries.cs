using Dapper;
using Bat.Core;
using System.Data.Common;
using CryptoGateway.Service.Models;
using CryptoGateway.Service.Handlers;
using CryptoGateway.Service.Resources;
using Microsoft.Extensions.Configuration;
using CryptoGateway.Service.Models.Payment;

namespace CryptoGateway.Service.Contracts.Query;

public static class PaymentQueries
{
    public static async Task<IResponse<object>> Query(
        this DbConnection connection, IConfiguration configuration, PaymentQueryModels.GetOwnersPayment query)
    {
        var baseUrl = $"{configuration["CustomSettings:ApplicationBaseUrl"]}/link/payment/";
        var result = await PaginationQueryHandler<PaymentReadModels.PaymentListItem>.CreateInstance(connection,
            selectClause:
                @"SELECT PaymentExternalId AS PaymentId, u.UserExternalId AS UserId, PriceAmount, CASE PriceCurrencyType WHEN 1 THEN 'IRR' WHEN 2 THEN 'USDT' END PriceCurrencyType, 
		             PayAmount, CASE PayCurrencyType WHEN 1 THEN 'IRR' WHEN 2 THEN 'USDT' END PayCurrencyType, 
		             p.CustomerId, OrderId, OrderDesc, @BaseUrl + CAST(PaymentExternalId AS VARCHAR(27)) AS PaymentLink,
		             FORMAT(p.[CreatedAt], 'yyyy-MM-dd hh:mm tt') AS CreatedAt,
                     FORMAT(p.ModifiedAt, 'yyyy-MM-dd hh:mm tt') AS ModifiedAt",
            fromClause:
                @"FROM [Payment].[Payment] p INNER JOIN
	                   [Base].[User] u ON p.UserId = u.UserId",
            whereClause:
                @"WHERE p.IsDeleted = 0 AND u.UserExternalId = @UserId",
            pagingClause:
                @"ORDER BY p.CreatedAt DESC
                  OFFSET @Offset ROWS
                  FETCH NEXT @PageSize ROWS ONLY",
            pagination: new PaginationModel
            {
                Page = query.Page,
                PageSize = query.PageSize
            },
            parameters: new DynamicParameters(new
            {
                UserId = query.UserId,
                BaseUrl = baseUrl
            })
        ).Handle();

        return Response<object>.Success(result, ServiceMessages.Success);
    }

    public static Task<IEnumerable<PaymentReadModels.WaitingPaymentsListItem>> Query(
        this DbConnection connection,
        PaymentQueryModels.GetWaitingPayments query)
        => connection.QueryAsync<PaymentReadModels.WaitingPaymentsListItem>(
            @"SELECT PaymentId, PaymentExternalId, u.UserId, u.Email AS UserEmail, u.MobileNumber AS UserMobileNumber, p.CustomerId,
	                 cua.ContractAccountId, PriceAmount, PriceCurrencyType, PayAmount, PayCurrencyType, [State] AS PaymentState, 
	                 OrderId, OrderDesc, coa.AddressBase58, coa.AddressHex, p.CreatedAt, p.ModifiedAt, ExpiredAt	  	   
             FROM [Payment].[Payment] p INNER JOIN [Payment].[CustodyAccount] cua ON 
	              p.CustodyAccountId = cua.CustodyAccountId INNER JOIN [Contract].[ContractAccount] coa ON
		          cua.ContractAccountId = coa.ContractAccountId INNER JOIN [Base].[User] u ON
		          p.UserId = u.UserId
             WHERE p.IsDeleted = 0 AND [State] = 1
             ORDER BY CreatedAt",
            new {});

    public static async Task<IResponse<object>> Query(
        this DbConnection connection, PaymentQueryModels.GetPaymentLinkInfo query)
    {
        var result = await connection.QueryAsync<PaymentReadModels.PaymentLinkInfo>(
            @"SELECT [PaymentExternalId] AS PaymentId
	                ,p.[CustomerId]	              
                    ,p.[CustomerContact]
	                ,[OrderId]
	                ,p.[OrderDesc]
	                ,[PriceAmount]
	                ,CASE PriceCurrencyType WHEN 1 THEN 'IRR' WHEN 2 THEN 'USDT' END PriceCurrencyType
	                ,[PayAmount]
	                ,CASE PayCurrencyType WHEN 1 THEN 'IRR' WHEN 2 THEN 'USDT' END PayCurrencyType
	                ,'TRC20' AS [Network]
                    ,coa.[AddressBase58] AS [Address]
	                ,FORMAT(p.[CreatedAt], 'yyyy-MM-dd hh:mm tt') AS CreatedAt
             FROM [Payment].[Payment] p INNER JOIN
	              [Payment].[CustodyAccount] cua ON p.CustodyAccountId = cua.CustodyAccountId INNER JOIN
	              [Contract].[ContractAccount] coa ON cua.[CustodyAccountId] = coa.[CustodyAccountId] INNER JOIN
	              [Base].[User] u ON p.UserId = u.UserId
             WHERE p.[IsDeleted] = 0 AND [PaymentExternalId] = @PaymentId -- AND [UserExternalId] = @UserId"
            ,
            new
            {
                //query.UserId,
                query.PaymentId
            });
        return Response<object>.Success(result.FirstOrDefault()!, ServiceMessages.Success);
    }
}