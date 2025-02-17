﻿using Microsoft.AspNetCore.Mvc;

namespace CryptoGateway.BoApi;

public static class RequestHandler
{
    public static async Task<IActionResult> HandleCommand<T, TResult>(
            string currentUserId, T request, Func<string, T, Task<TResult>> handler, Serilog.ILogger log)
    //T request, Func<T, Task> handler, Serilog.ILogger log)
    {
        try
        {
            log.Debug("Handling HTTP request of type {type}", typeof(T).Name);
            //await handler(request);
            //return new OkResult();
            return new OkObjectResult(await handler(currentUserId, request));
        }
        catch (Exception e)
        {
            log.Error(e, "Error handling the command");
            return new BadRequestObjectResult(new
            {
                error = e.Message,
                stackTrace = e.StackTrace
            });
        }
    }

    public static async Task<IActionResult> HandleQuery<TModel>(
        Func<Task<TModel>> query, Serilog.ILogger log)
    {
        try
        {
            return new OkObjectResult(await query());
        }
        catch (Exception e)
        {
            log.Error(e, "Error handling the query");
            return new BadRequestObjectResult(new
            {
                error = e.Message,
                stackTrace = e.StackTrace
            });
        }
    }
}