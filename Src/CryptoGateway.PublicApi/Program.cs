using Asp.Versioning;
using System.Data.Common;
using CryptoGateway.Framework;
using Microsoft.Data.SqlClient;
using Asp.Versioning.Conventions;
using Microsoft.EntityFrameworkCore;
using CryptoGateway.Service.Implements;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.Domain.Entities.PayIn;
using CryptoGateway.DataAccess.DbContexts;
using CryptoGateway.DomainService.Payment;
using CryptoGateway.DataAccess.UnitOfWorks;
using CryptoGateway.Domain.Entities.Shared;
using CryptoGateway.Domain.Entities.Payout;
using CryptoGateway.Domain.Entities.ApiKey;
using CryptoGateway.DataAccess.Repositories;
using CryptoGateway.Domain.Entities.Payment;
using CryptoGateway.PublicApi.Infrastructure;
using CryptoGateway.Domain.Entities.UserCredit;
using CryptoGateway.Domain.Entities.Notification;
using CryptoGateway.Domain.Entities.CustodyAccount;
using CryptoGateway.Domain.Entities.ContractAccount;
using CryptoGateway.PublicApi.Infrastructure.ApiKey;
using CryptoGateway.Service.ExternalWebServices.RamzPlus;
using CryptoGateway.Service.ExternalWebServices.NodeJsApi;
using CryptoGateway.DomainService.ExternalWebServices.NodeJsApi;
using CryptoGateway.DomainService.ExternalWebServices.RamzPlus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;

    // reporting api versions will return the headers
    // "api-supported-versions" and "api-deprecated-versions"
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("x-version"));
}).AddMvc(
    options =>
    {
        // automatically applies an api version based on the name of
        // the defining controller's namespace
        options.Conventions.Add(new VersionByNamespaceConvention());
    });

builder.Services.AddDbContext<AppDbContext>(option =>
    option.UseSqlServer(configuration.GetConnectionString("AppDbContext")));
builder.Services.AddScoped<DbConnection>(c =>
    new SqlConnection(configuration.GetConnectionString("AppDbContext")));
builder.Services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IApiKeyValidation, ApiKeyValidation>();
builder.Services.AddScoped<ApiKeyAuthFilter>();

builder.Services.AddScoped<ISpotPriceProvider, FixedSpotPrice>();
builder.Services.AddScoped<IBlockedCredit, FixedBlockedCredit>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPayoutRepository, PayoutRepository>();
builder.Services.AddScoped<IPayInRepository, PayInRepository>();
builder.Services.AddScoped<ICustodyAccRepository, CustodyAccRepository>();
builder.Services.AddScoped<IContractAccountRepository, ContractAccountRepository>();
builder.Services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IUserCreditDomainService, UserCreditDomainService>();
builder.Services.AddScoped<IContractAccDomainService, ContractAccountDomainService>();
builder.Services.AddScoped<PaymentApplicationService>();
builder.Services.AddScoped<ITronWeb, TronWeb>();
builder.Services.AddScoped<IRamzPlusPublicApi, RamzPlusPublicApi>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
