using CryptoGateway.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CryptoGateway.WebApi.Infrastructure;

public static class AppBuilderDatabaseExtensions
{
    public static IApplicationBuilder EnsureDatabase(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();
        EnsureContextIsMigrated(scope.ServiceProvider.GetRequiredService<AppDbContext>());
        //            EnsureContextIsMigrated(builder.ApplicationServices.GetService<UserProfileDbContext>());
        return builder;
    }

    private static void EnsureContextIsMigrated(DbContext context)
    {
        if (!context.Database.EnsureCreated())
            context.Database.Migrate();
    }

    public static IServiceCollection AddSqlDbContext<T>(this IServiceCollection services,
        string connectionString) where T : DbContext
    {
        services.AddDbContext<T>(options => options.UseSqlServer(connectionString));
        return services;
    }
}