using Microsoft.EntityFrameworkCore;
using CryptoGateway.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoGateway.DataAccess.AppConfigurations.Auth;

public class MenuConfig : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable(nameof(Menu), "Auth");

        builder.Property(x => x.MenuId)
            .UseIdentityColumn();

        builder.Property(x => x.ShowInMenu)
            .HasDefaultValue(true);

        builder.Property(x => x.Icon)
            .HasColumnType("varchar")
            .HasMaxLength(40);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Path)
            .HasColumnType("varchar")
            .HasMaxLength(100);

        builder
            .HasMany(x => x.ChildMenus)
            .WithOne(x => x.Parent)
            .HasForeignKey(x => x.ParentId)
            .IsRequired(false);

        builder
            .HasMany(x => x.MenuInRoles)
            .WithOne(x => x.Menu)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}