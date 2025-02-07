using Dapper;
using Bat.Core;
using System.Data.Common;
using CryptoGateway.Service.Models;
using CryptoGateway.Service.Handlers;
using CryptoGateway.Service.Resources;
using Microsoft.Extensions.Configuration;
using CryptoGateway.Service.Models.CustodyAccount;

namespace CryptoGateway.Service.Contracts.Query;

public static class CustodyAccountQueries
{
    public static Task<IEnumerable<CustodyAccReadModels.CustodyAccAllItems>> Query(
        this DbConnection connection,
        CustodyAccQueryModels.GetAllCustodyAccounts query)
        => connection.QueryAsync<CustodyAccReadModels.CustodyAccAllItems>(
            @"SELECT cua.CustodyAccountId, coa.ContractAccountId, cua.UserId, u.Email AS UserEmail, u.MobileNumber AS userMobileNumber, cua.CustomerId,  
                     CustomerContact = (SELECT TOP 1 CustomerContact 
						                FROM [Payment].[Payment] p 
						                WHERE p.CustodyAccountId = cua.CustodyAccountId AND p.CustomerId = cua.CustomerId  
						                ORDER BY CreatedAt DESC),
	                 cua.CreatedAt	
             FROM [Payment].[CustodyAccount] cua INNER JOIN
                  [Contract].[ContractAccount] coa ON cua.CustodyAccountId = coa.CustodyAccountId INNER JOIN
	              [Base].[User] u ON cua.UserId = u.UserId",
            new { });
}