using Serilog;
using System.Text;
using Asp.Versioning;
using System.Data.Common;
using CryptoGateway.Framework;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;
using Asp.Versioning.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using CryptoGateway.Service.Implements;
using CryptoGateway.Service.JwtFeatures;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.WebApi.Infrastructure;
using CryptoGateway.DomainService.Payment;
using CryptoGateway.DataAccess.DbContexts;
using CryptoGateway.Domain.Entities.PayIn;
using CryptoGateway.DataAccess.UnitOfWorks;
using CryptoGateway.Domain.Entities.Wallet;
using CryptoGateway.Domain.Entities.Shared;
using CryptoGateway.Domain.Entities.Payout;
using CryptoGateway.Domain.Entities.ApiKey;
using CryptoGateway.DataAccess.Repositories;
using CryptoGateway.Domain.Entities.Payment;
using CryptoGateway.Domain.Entities.Currency;
using CryptoGateway.Domain.Entities.UserCredit;
using CryptoGateway.Domain.Entities.BankAccount;
using CryptoGateway.Domain.Entities.Notification;
using CryptoGateway.Domain.Entities.CustodyAccount;
using CryptoGateway.Domain.Entities.ContractAccount;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CryptoGateway.Service.ExternalWebServices.RamzPlus;
using CryptoGateway.Service.ExternalWebServices.NodeJsApi;
using CryptoGateway.DomainService.ExternalWebServices.NodeJsApi;
using CryptoGateway.DomainService.ExternalWebServices.RamzPlus;

//using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;

var allowedOrigins = "MyValidOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(allowedOrigins, builder =>
    {
        builder.WithOrigins(configuration.GetSection("AllowedOrigin").Value.Split(";"))
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
            //.WithExposedHeaders("Content-Disposition");
    });
});

Log.Logger = new LoggerConfiguration()
    //.ReadFrom.Settings(configuration)
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;

    // reporting api versions will return the headers
    // "api-supported-versions" and "api-deprecated-versions"
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
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
//builder.Services.AddIdentity<User, Role>()
//    .AddUserStore<AppDbContext>()
//    .AddDefaultTokenProviders();
builder.Services.AddScoped<DbConnection>(c => 
    new SqlConnection(configuration.GetConnectionString("AppDbContext")));
//builder.Services.AddScoped<ICurrencyLookup, FixedCurrencyLookup>();
builder.Services.AddScoped<ISpotPriceProvider, FixedSpotPrice>();
builder.Services.AddScoped<IBlockedCredit, FixedBlockedCredit>();
builder.Services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();

builder.Services.AddHttpContextAccessor();
//builder.Services.AddTransient<IApiKeyValidation, ApiKeyValidation>();
//builder.Services.AddScoped<ApiKeyAuthFilter>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IPayoutRepository, PayoutRepository>();
builder.Services.AddScoped<IPayInRepository, PayInRepository>();
builder.Services.AddScoped<ICustodyAccRepository, CustodyAccRepository>();
builder.Services.AddScoped<IContractAccountRepository, ContractAccountRepository>();
builder.Services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<AuthApplicationService>();
builder.Services.AddScoped<UserApplicationService>();
builder.Services.AddScoped<PaymentApplicationService>();
builder.Services.AddScoped<BankAccountApplicationService>();
builder.Services.AddScoped<WalletApplicationService>();
builder.Services.AddScoped<PayoutApplicationService>();
builder.Services.AddScoped<PayInApplicationService>();

builder.Services.AddScoped<IUserCreditDomainService, UserCreditDomainService>();
builder.Services.AddScoped<IContractAccDomainService, ContractAccountDomainService>();
builder.Services.AddScoped<ITronWeb, TronWeb>();
builder.Services.AddScoped<IRamzPlusPublicApi, RamzPlusPublicApi>();

builder.Services.AddAutoMapper(typeof(Program));  //TODO: Remove AutoMapper in the future.

builder.Services.AddIdentity<User, IdentityRole<int>>(opt =>
    {
        opt.Password.RequiredLength = 7;
        opt.Password.RequireDigit = false;

        opt.User.RequireUniqueEmail = true;

        opt.Lockout.AllowedForNewUsers = false;
        opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
        opt.Lockout.MaxFailedAccessAttempts = 3;
    }).AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
    opt.TokenLifespan = TimeSpan.FromHours(2));

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["validIssuer"],
        ValidAudience = jwtSettings["validAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["securityKey"]!)),
        LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow
    };
});

builder.Services.AddAuthentication()
    .AddGoogle("google", options =>
    {
        var googleAuth = builder.Configuration.GetSection("Authentication:Google");

        options.ClientId = googleAuth["ClientId"]!;
        options.ClientSecret = googleAuth["ClientSecret"]!;
        options.SignInScheme = IdentityConstants.ExternalScheme;
    });

builder.Services.AddScoped<JwtHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "CryptoGateway.WebApi", Version = "V1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.EnsureDatabase();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoGateway.WebApi V1"));
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors(allowedOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
