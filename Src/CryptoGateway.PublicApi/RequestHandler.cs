using Bat.Core;
using CryptoGateway.Service.Resources;

namespace CryptoGateway.PublicApi;

public static class RequestHandler
{
    public static async Task<IResponse<object>> HandleCommand<T>(
            string currentUserId, T request, Func<string, T, Task<IResponse<object>>> handler, Serilog.ILogger log)
    {
        try
        {
            log.Debug("Handling HTTP request of type {type}", typeof(T).Name);
            return await handler(currentUserId, request);
        }
        catch (Exception e)
        {
            log.Error(e, "Error handling the command");
            return Response<object>.Error(ServiceMessages.UnexpectedError);
        }
    }

    public static async Task<IResponse<object>> HandleQuery(
        Func<Task<IResponse<object>>> query, Serilog.ILogger log)
    {
        try
        {
            return await query();
        }
        catch (Exception e)
        {
            log.Error(e, "Error handling the query");
            return Response<object>.Error(ServiceMessages.UnexpectedError);
        }
    }
}