using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CryptoGateway.WebApi.Filters;

public class SwaggerAuthenticateFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor descriptor)
        {
            if (!context.ApiDescription.CustomAttributes().Any((a) => a is AllowAnonymousAttribute)
                && descriptor.ControllerTypeInfo.GetCustomAttribute<AuthenticateFilter>() != null)
            {
                if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Token",
                    In = ParameterLocation.Header,
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Default = new OpenApiString("123")
                    },
                    Description = "Header Token For Authenticate Request.",
                });
            }
        }
    }
}