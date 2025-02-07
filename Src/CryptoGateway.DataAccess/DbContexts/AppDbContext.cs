using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.Domain.Entities.PayIn;
using CryptoGateway.Domain.Entities.Wallet;
using CryptoGateway.Domain.Entities.Payout;
using CryptoGateway.Domain.Entities.ApiKey;
using CryptoGateway.Domain.Entities.Payment;
using CryptoGateway.Domain.Entities.Currency;
using CryptoGateway.Domain.Entities.UserCredit;
using CryptoGateway.Domain.Entities.BankAccount;
using CryptoGateway.Domain.Entities.Transaction;
using CryptoGateway.Domain.Entities.Notification;
using CryptoGateway.Domain.Entities.CustodyAccount;
using CryptoGateway.Domain.Entities.ContractAccount;
using CryptoGateway.DataAccess.DbContexts.Extensions;
using CryptoGateway.Domain.Entities.CurrencySpotPrice;
using CryptoGateway.DataAccess.AppConfigurations.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CryptoGateway.Domain.Entities.ContractTransaction;
using CryptoGateway.DataAccess.AppConfigurations.Payment;
using CryptoGateway.DataAccess.AppConfigurations.Contract;
using CryptoGateway.DataAccess.AppConfigurations.Messaging;
using CryptoGateway.DataAccess.AppConfigurations.Transaction;
using CryptoGateway.DataAccess.AppConfigurations.CustodyAccount;

namespace CryptoGateway.DataAccess.DbContexts;

public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int> //BatDbContext
{
    private readonly ILoggerFactory _loggerFactory;

    public AppDbContext(DbContextOptions options, ILoggerFactory loggerFactory)
        : base(options)
    {
        _loggerFactory = loggerFactory;
    }

    public DbSet<User> User { get; set; }
    public DbSet<Payment> Payment { get; set; }
    public DbSet<Currency> Currency { get; set; }
    public DbSet<BankAccount> BankAccount { get; set; }
    public DbSet<Wallet> Wallet { get; set; }
    public DbSet<Payout> Payout { get; set; }
    public DbSet<PayIn> PayIn { get; set; }
    public DbSet<UserCredit> UserCredit { get; set; }
    public DbSet<CustodyAccount> CustodyAccount { get; set; }
    public DbSet<ContractAccount> ContractAccount { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<ApiKey> ApiKey { get; set; }
    public DbSet<Notification> Notification { get; set; }
    public DbSet<ContractTransaction> ContractTransaction { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory);

        //TODO: NOTE! Be sure to disable sensitive data logging when deploying to production.
        //optionsBuilder.EnableSensitiveDataLogging();
        //optionsBuilder.EnableDetailedErrors();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.OverrideDeleteBehavior(DeleteBehavior.NoAction);
        //modelBuilder.RegisterAllEntities<IEntity>(typeof(Role).Assembly);
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(RoleConfig).Assembly);

        modelBuilder.Entity<IdentityRole<int>>().ToTable("Role", "Identity");
        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaim", "Identity");
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRole", "Identity");
        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogin", "Identity");
        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserToken", "Identity");
        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaim", "Identity");

        modelBuilder.ApplyConfiguration(new UserConfig());
        modelBuilder.ApplyConfiguration(new PaymentConfig());
        modelBuilder.ApplyConfiguration(new CurrencyConfig());
        modelBuilder.ApplyConfiguration(new BankAccountConfig());
        modelBuilder.ApplyConfiguration(new WalletConfig());
        modelBuilder.ApplyConfiguration(new PayoutConfig());
        modelBuilder.ApplyConfiguration(new PayInConfig());
        modelBuilder.ApplyConfiguration(new UserCreditConfig());
        modelBuilder.ApplyConfiguration(new CustodyAccountConfig());
        modelBuilder.ApplyConfiguration(new ContractAccountConfig());
        modelBuilder.ApplyConfiguration(new TransactionConfig());
        modelBuilder.ApplyConfiguration(new ApiKeyConfig());
        modelBuilder.ApplyConfiguration(new NotificationConfig());
        modelBuilder.ApplyConfiguration(new ContractTransactionConfig());

        modelBuilder.Ignore<CurrencySpotPrice>();
    }
}