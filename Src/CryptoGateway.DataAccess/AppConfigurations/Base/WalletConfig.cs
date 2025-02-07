using Microsoft.EntityFrameworkCore;
using CryptoGateway.Domain.Entities.Wallet;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CryptoGateway.Domain.Entities.Wallet.ValueObjects;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.DataAccess.AppConfigurations.Base;

public class WalletConfig : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable(nameof(Wallet), "Base");

        builder.HasKey(x => x.WalletId);

        builder.Property(x => x.WalletId)
            .HasConversion(v => v.Value, v => new WalletId(v))
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();
        
        builder.OwnsOne(x => x.Id).Ignore(x => x.Value);

        builder.OwnsOne(x => x.WalletTitle)
            .Property(x => x.Value)
            .IsRequired()
            .HasColumnName("Title")
            .HasColumnType("nvarchar")
            .HasMaxLength(50);

        builder.Property(x => x.State)
            .IsRequired()
            .HasColumnName("State")
            .HasColumnType("tinyint");

        builder.Property(e => e.CurrencyId)
            .HasConversion(v => v.Value, v => new CurrencyId(v));

        builder.OwnsOne(x => x.Network)
            .Property(x => x.Value)
            .IsRequired()
            .HasColumnName("Network")
            .HasColumnType("varchar")
            .HasMaxLength(10);

        builder.Property(x => x.Address)
            .HasConversion(v => v.Value, v => WalletAddress.FromString(v))
            .IsRequired()
            .HasColumnName("Address")
            .HasColumnType("varchar")
            .HasMaxLength(100);

        builder.OwnsOne(x => x.MemoAddress)
            .Property(x => x.Value)
            .IsRequired(false)
            .HasColumnName("Memo")
            .HasColumnType("varchar")
            .HasMaxLength(100);

        builder.OwnsOne(x => x.TagAddress)
            .Property(x => x.Value)
            .IsRequired(false)
            .HasColumnName("Tag")
            .HasColumnType("varchar")
            .HasMaxLength(100);

        builder.OwnsOne(x => x.Desc)
            .Property(x => x.Value)
            .IsRequired(false)
            .HasColumnName("Desc")
            .HasColumnType("nvarchar")
            .HasMaxLength(70);

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