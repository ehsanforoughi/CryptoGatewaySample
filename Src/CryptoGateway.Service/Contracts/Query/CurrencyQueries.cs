using Dapper;
using Bat.Core;
using System.Data.Common;
using CryptoGateway.Service.Resources;
using CryptoGateway.Service.Models.Currency;

namespace CryptoGateway.Service.Contracts.Query;

public static class CurrencyQueries
{
    public async static Task<IResponse<object>> Query(
        this DbConnection connection)
    {
        var result = connection.QueryAsync<CurrencyReadModels.CurrencyListItem>(
            "SELECT CurrencyId, Code AS CurrencyCode, DecimalPlaces, IsActive AS InUse " +
            "FROM Base.Currency");

        return Response<object>.Success(result, ServiceMessages.Success);
    }
}