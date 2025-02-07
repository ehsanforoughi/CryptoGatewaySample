using Bat.Core;
using System.Net;
using System.Text;
using Bat.AspNetCore;
using Microsoft.Extensions.Options;

namespace CryptoGateway.BoApi.Middlewares;

public class JwtParserMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;

    public JwtParserMiddleware(RequestDelegate next,
        IJwtService jwtService, IOptions<JwtSettings> jwtSettings)
    {
        _next = next;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        try
        {
            if (token != null)
            {
                if (_jwtService == null)
                {
                    var response = Encoding.UTF8.GetBytes(new
                    {
                        resultCode = 1003,
                        isSuccess = false,
                        message = "Jwt Service Not Configure !"
                    }.SerializeToJson());

                    context.Response.ContentType = "application/Json";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await context.Response.Body.WriteAsync(response);
                }

                var userClaims = _jwtService.GetClaimsPrincipal(token, _jwtSettings);
                if (userClaims != null) context.User = userClaims;
            }
            else
            {
                if (context.User.Claims.Any())
                {
                    context.Response.ContentType = "application/Json";
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    var bytes = Encoding.UTF8.GetBytes(new
                    {
                        resultCode = 1004,
                        isSuccess = false,
                        message = "UnAuthorized Access To Api !. Token Not Sent.",
                    }.SerializeToJson());
                    await context.Response.Body.WriteAsync(bytes);
                }
            }

            await _next(context);
        }
        catch (Exception e)
        {
            byte[] response;
            if (e.Message.Contains("Lifetime validation failed"))
            {
                #region Expired Token
                response = Encoding.UTF8.GetBytes(new
                {
                    resultCode = 1001,
                    isSuccess = false,
                    message = "توکن اعتبار سنجی شما به پایان رسیده است، لطفا مجددا وارد سامانه شوید."
                }.SerializeToJson());
                #endregion
            }
            else
            {
                FileLoger.Error(e);

                #region Another Exception
                response = Encoding.UTF8.GetBytes(new
                {
                    resultCode = 1002,
                    isSuccess = false,
                    message = "عملیات مورد نظر در سرور با خطا مواجه شده است، لطفا مجددا اقدام نمایید."
                }.SerializeToJson());
                #endregion
            }

            context.Response.ContentType = "application/Json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.Body.WriteAsync(response);
        }
    }
}