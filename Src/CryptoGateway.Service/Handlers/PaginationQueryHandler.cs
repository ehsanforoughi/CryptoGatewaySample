using Dapper;
using Bat.Core;
using System.Data.Common;
using CryptoGateway.Service.Models;

namespace CryptoGateway.Service.Handlers;

public class PaginationQueryHandler<T> where T : class
{
    private readonly DbConnection _connection;
    private readonly PaginationModel _pagination;
    private readonly DynamicParameters _parameters;

    private PaginationQueryHandler(DbConnection connection,
        string selectClause, string fromClause, 
        string whereClause, string pagingClause, 
        PaginationModel pagination, DynamicParameters parameters)
    {
        _connection = connection;
        _pagination = pagination;
        _parameters = parameters;
        SelectClause = selectClause;
        FromClause = fromClause;
        WhereClause = whereClause;
        PagingClause = pagingClause;
    }

    public static PaginationQueryHandler<T> CreateInstance(
        DbConnection connection, string selectClause, 
        string fromClause, string whereClause, string pagingClause, 
        PaginationModel pagination, DynamicParameters parameters)
    {
        return new PaginationQueryHandler<T>(connection, selectClause, 
            fromClause, whereClause, pagingClause, pagination, parameters);
    }
    public string SelectClause { get; set; }
    public string FromClause { get; set; }
    public string WhereClause { get; set; }
    public string PagingClause { get; set; }

    public async Task<object> Handle()
    {
        var count = $@"SELECT COUNT(*)
                      {FromClause}
                      {WhereClause}";

        var totalCount = await _connection.ExecuteScalarAsync<int>(count, _parameters);

        _parameters.Add("PageSize", _pagination.PageSize);
        _parameters.Add("Offset", _pagination.Offset);

        var items = await _connection.QueryAsync<T>(
            $@"{SelectClause}
               {FromClause}
               {WhereClause}
               {PagingClause}", 
            _parameters);

        return items.ToPagingListDetails(totalCount, new PagingParameter(_pagination.Page, _pagination.PageSize));
    }
}