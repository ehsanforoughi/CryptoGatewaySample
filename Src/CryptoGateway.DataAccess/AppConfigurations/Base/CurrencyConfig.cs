using Microsoft.EntityFrameworkCore;
using CryptoGateway.Domain.Entities.Currency;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Currency.ValueObjects;

namespace CryptoGateway.DataAccess.AppConfigurations.Base;

public class CurrencyConfig : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable(nameof(Currency), "Base");

        builder.HasKey(x => x.CurrencyId);

        builder.Property(x => x.CurrencyId)
            .HasConversion(v => v.Value, v => new CurrencyId(v))
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.OwnsOne(x => x.Id)
            .Ignore(x => x.Value);

        builder.Property(x => x.Code)
            .HasConversion(v => v.Value, v => CurrencyCode.FromString(v))
            .IsRequired()
            .HasColumnName("Code")
            .HasColumnType("varchar")
            .HasMaxLength(10);

        builder.Property(x => x.Type)
            .IsRequired()
            .HasColumnType("tinyint");

        builder.OwnsOne(x => x.FullName)
            .Property(x => x.Value)
            .IsRequired()
            .HasColumnName("FullName")
            .HasColumnType("varchar")
            .HasMaxLength(50);

        builder.OwnsOne(x => x.NameFa)
            .Property(x => x.Value)
            .IsRequired()
            .HasColumnName("NameFa")
            .HasColumnType("nvarchar")
            .HasMaxLength(30);

        builder.OwnsOne(x => x.IsFiat)
            .Property(x => x.Value)
            .IsRequired()
            .HasColumnName("IsFiat");

        builder.OwnsOne(x => x.Priority)
            .Property(x => x.Value)
            .IsRequired()
            .HasColumnName("Priority");

        builder.OwnsOne(x => x.IsActive)
            .Property(x => x.Value)
            .IsRequired()
            .HasColumnName("IsActive");

        builder.OwnsOne(x => x.LogoUrl)
            .Property(x => x.Value)
            .IsRequired(false)
            .HasColumnName("LogoUrl");

        builder.OwnsOne(x => x.Network)
            .Property(x => x.Value)
            .IsRequired(false)
            .HasColumnName("Network")
            .HasColumnType("varchar")
            .HasMaxLength(50);

        builder.OwnsOne(x => x.DecimalPlaces)
            .Property(x => x.Value)
            .IsRequired()
            .HasColumnName("DecimalPlaces");

        builder.OwnsOne(x => x.IsTradable)
            .Property(x => x.Value)
            .IsRequired()
            .HasColumnName("IsTradable");

        builder.OwnsOne(x => x.IsDepositable)
            .Property(x => x.Value)
            .IsRequired()
            .HasColumnName("IsDepositable");

        builder.OwnsOne(x => x.IsWithdrawable)
            .Property(x => x.Value)
            .IsRequired()
            .HasColumnName("IsWithdrawable");

        builder.OwnsOne(x => x.NetworkTransferFee)
            .Property(x => x.Value)
            .IsRequired()
            .HasColumnName("NetworkTransferFee")
            .HasColumnType("decimal(24,12)");

        builder.OwnsOne(x => x.MinimumAmount)
            .Property(x => x.Value)
            .IsRequired()
            .HasColumnName("MinimumAmount")
            .HasColumnType("decimal(24,12)");

        builder.OwnsOne(x => x.MinimumDeposit)
            .Property(x => x.Value)
            .IsRequired()
            .HasColumnName("MinimumDeposit")
            .HasColumnType("decimal(24,12)");

        builder.OwnsOne(x => x.MinimumWithdraw)
            .Property(x => x.Value)
            .IsRequired()
            .HasColumnName("MinimumWithdraw")
            .HasColumnType("decimal(24,12)");

        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.InsertDateMi)
            .HasColumnName("CreatedAt")
            .IsRequired();

        builder.Property(x => x.ModifyDateMi)
            .HasColumnName("ModifiedAt")
            .IsRequired();
    }
}