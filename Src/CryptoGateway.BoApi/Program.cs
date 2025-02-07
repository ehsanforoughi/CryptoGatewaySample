using Serilog;
using System.Data.Common;
using CryptoGateway.Framework;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using CryptoGateway.Service.Implements;
using CryptoGateway.BoApi.Infrastructure;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.DataAccess.DbContexts;
using CryptoGateway.DomainService.Payment;
using CryptoGateway.DataAccess.UnitOfWorks;
using CryptoGateway.Domain.Entities.Shared;
using CryptoGateway.Domain.Entities.Wallet;
using CryptoGateway.Domain.Entities.Payout;
using CryptoGateway.DataAccess.Repositories;
using CryptoGateway.Domain.Entities.Payment;
using CryptoGateway.Domain.Entities.Currency;
using CryptoGateway.Domain.Entities.UserCredit;
using CryptoGateway.Domain.Entities.BankAccount;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
Log.Logger = new LoggerConfiguration()
    //.ReadFrom.Settings(configuration)
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddDbContext<AppDbContext>(option =>
    option.UseSqlServer(configuration.GetConnectionString("AppDbContext")));

builder.Services.AddScoped<DbConnection>(c => new SqlConnection(configuration.GetConnectionString("AppDbContext")));
//builder.Services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
builder.Services.AddSingleton<ISpotPriceProvider, FixedSpotPrice>();
builder.Services.AddScoped<IBlockedCredit, FixedBlockedCredit>();
builder.Services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IPayoutRepository, PayoutRepository>();
builder.Services.AddScoped<PayoutApplicationService>();
builder.Services.AddScoped<IUserCreditDomainService, UserCreditDomainService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

//app.UseMiddleware<JwtParserMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
