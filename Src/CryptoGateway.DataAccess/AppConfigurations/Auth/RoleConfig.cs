using Microsoft.EntityFrameworkCore;
using CryptoGateway.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoGateway.DataAccess.AppConfigurations.Auth;

public class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(nameof(Role), "Auth");

        builder.Property(x => x.RoleId)
            .UseIdentityColumn();

        builder.Property(x => x.Type)
            .HasDefaultValue(RoleType.User);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.RoleNameFa)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.RoleNameEn)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(30);

        builder
            .HasMany(x => x.MenuInRoles)
            .WithOne(x => x.Role)
            .HasForeignKey(x => x.RoleId)
            .IsRequired();

        builder
            .HasMany(x => x.UserInRoles)
            .WithOne(x => x.Role)
            .HasForeignKey(x => x.RoleId)
            .IsRequired();
    }
}