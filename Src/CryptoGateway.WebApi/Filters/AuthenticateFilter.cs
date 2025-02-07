using Bat.Core;
using Bat.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace CryptoGateway.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class AuthenticateFilter : ActionFilterAttribute, IAsyncActionFilter
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
    {
        try
        {
            var ActionDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
            bool skipAuthenticate = ActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true).Any(a => a.GetType().Equals(typeof(AllowAnonymousAttribute)));
            if (!skipAuthenticate)
            {
                var ip = ClientInfo.GetIP(filterContext.HttpContext);
                var _configuration = (filterContext.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration);
                if (filterContext.HttpContext.Request.Headers["Token"].Count > 0)
                {
                    #region Existing Token
                    if (int.TryParse(filterContext.HttpContext.Request.Headers["Token"][0], out int token))
                    {
                        if (token != int.Parse(_configuration["CustomSettings:AuthenticateToken"]))
                        {
                            FileLoger.Info($"Invalid Token To Access Api  !" + Environment.NewLine +
                                $"IP: {ip}" + Environment.NewLine +
                                $"Token:{filterContext.HttpContext.Request.Headers["Token"][0]}" + Environment.NewLine +
                                $"URL:{filterContext.HttpContext.Request.Path.Value}" + Environment.NewLine +
                                $"UrlReferer:{filterContext.HttpContext.Request.GetTypedHeaders().Referer}");

                            filterContext.HttpContext.Response.StatusCode = 401;
                            filterContext.Result = new JsonResult(new Response<object>
                            {
                                Result = 401,
                                IsSuccess = false,
                                Message = "UnAuthorized Access. Invalid Token To Access Api."
                            });
                        }
                    }
                    else
                    {
                        FileLoger.Info($"Invalid Token Type To Access Api  !" + Environment.NewLine +
                                $"IP: {ip}" + Environment.NewLine +
                                $"Token:{filterContext.HttpContext.Request.Headers["Token"][0]}" + Environment.NewLine +
                                $"URL:{filterContext.HttpContext.Request.Path.Value}" + Environment.NewLine +
                                $"UrlReferer:{filterContext.HttpContext.Request.GetTypedHeaders().Referer}");

                        filterContext.HttpContext.Response.StatusCode = 401;
                        filterContext.Result = new JsonResult(new Response<object>
                        {
                            ResultCode = 401,
                            IsSuccess = false,
                            Message = "UnAuthorized Access. Invalid Token Type To Access Api  !"
                        });
                    }
                    #endregion
                }
                else
                {
                    #region Not Existing Token
                    FileLoger.Info($"UnAuthorized Access To Api. Token Not Sent." + Environment.NewLine +
                        $"IP: {ip}" + Environment.NewLine +
                        $"URL:{filterContext.HttpContext.Request.Path.Value}" + Environment.NewLine +
                        $"UrlReferer:{filterContext.HttpContext.Request.GetTypedHeaders().Referer}");

                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.Result = new JsonResult(new Response<object>
                    {
                        ResultCode = 401,
                        IsSuccess = false,
                        Message = "UnAuthorized Access. Token Not Sent."
                    });
                    #endregion
                }
            }

            await base.OnActionExecutionAsync(filterContext, next);
        }
        catch (Exception e)
        {
            FileLoger.Error(e);
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.Result = new JsonResult(new Response<object>
            {
                ResultCode = 500,
                IsSuccess = false,
                Message = "Internall Error." + e.Message
            });

            await base.OnActionExecutionAsync(filterContext, next);
        }
    }
}