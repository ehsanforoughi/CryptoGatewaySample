using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CryptoGateway.DataAccess.DbContexts.Extensions;

public static class ModelBuilderExtensions
{
    public static void OverrideDeleteBehavior(this ModelBuilder modelBuilder, DeleteBehavior deleteBehaviour = DeleteBehavior.Restrict)
    {
        foreach (IMutableForeignKey item in modelBuilder.Model.GetEntityTypes().SelectMany((e) => e.GetForeignKeys()))
        {
            item.DeleteBehavior = deleteBehaviour;
        }
    }
}