using Microsoft.EntityFrameworkCore;
using CryptoGateway.Domain.Entities.Payout;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CryptoGateway.Domain.Entities.Payout.ValueObjects;
using CryptoGateway.Domain.Entities.Wallet.ValueObjects;
using CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

namespace CryptoGateway.DataAccess.AppConfigurations.Payment;

public class PayoutConfig : IEntityTypeConfiguration<Payout>
{
    public void Configure(EntityTypeBuilder<Payout> builder)
    {
        builder.ToTable(nameof(Payout), "Payment");

        builder.HasKey(x => x.PayoutId);

        builder.Property(x => x.PayoutId)
            .HasConversion(v => v.Value, v => new PayoutId(v))
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.OwnsOne(x => x.Id)
            .Ignore(x => x.Value);

        builder.Property(x => x.TransferType)
            .IsRequired()
            .HasColumnName("TransferType")
            .HasColumnType("tinyint");

        builder.Property(x => x.State)
            .IsRequired()
            .HasColumnName("State")
            .HasColumnType("tinyint");

        builder.OwnsOne(x => x.Value, p =>
        {
            p.Property(x => x.Amount)
                .HasColumnName("Value")
                .HasColumnType("decimal(24,12)");

            p.Property(x => x.CurrencyType)
                .HasColumnName("CurrencyType")
                .HasColumnType("tinyint");
        });

        builder.Property(e => e.BankAccountId)
            .HasConversion(v => v.Value, v => new BankAccountId(v))
            .IsRequired(false);

        builder.Property(e => e.WalletId)
            .HasConversion(v => v.Value, v => new WalletId(v))
            .IsRequired(false);

        builder.OwnsOne(x => x.BankTrackingCode)
            .Property(x => x.Value)
            .IsRequired(false)
            .HasColumnName("BankTrackingCode");

        builder.OwnsOne(x => x.TransactionUrl)
            .Property(x => x.Value)
            .IsRequired(false)
            .HasColumnName("TransactionUrl");

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