using Microsoft.EntityFrameworkCore;
using CryptoGateway.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoGateway.DataAccess.AppConfigurations.Auth;

public class MenuInRoleConfig : IEntityTypeConfiguration<MenuInRole>
{
    public void Configure(EntityTypeBuilder<MenuInRole> builder)
    {
        builder.ToTable(nameof(MenuInRole), "Auth");

        builder.Property(x => x.MenuInRoleId)
            .UseIdentityColumn();

        builder.Property(x => x.IsDefault)
            .HasDefaultValue(false);
    }
}