using Microsoft.EntityFrameworkCore;
using CryptoGateway.Domain.Entities.PayIn;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CryptoGateway.Domain.Entities.PayIn.ValueObjects;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.DataAccess.AppConfigurations.Payment;

public class PayInConfig : IEntityTypeConfiguration<PayIn>
{
    public void Configure(EntityTypeBuilder<PayIn> builder)
    {
        builder.ToTable(nameof(PayIn), "Payment");

        builder.HasKey(x => x.PayInId);

        builder.Property(x => x.PayInId)
            .HasConversion(v => v.Value, v => new PayInId(v))
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.OwnsOne(x => x.Id)
            .Ignore(x => x.Value);

        builder.OwnsOne(x => x.PayInExternalId, p =>
        {
            p.Property(x => x.Value)
                .HasMaxLength(27)
                .HasColumnName("PayInExternalId");

            p.HasIndex(x => x.Value)
                .IsUnique();
        });

        builder.Property(e => e.UserId)
            .IsRequired();

        builder.Property(e => e.CustomerId)
            .HasConversion(v => v.Value, v => new CustomerId(v))
            .HasColumnName("CustomerId")
            .IsRequired();

        builder.OwnsOne(x => x.Value, p =>
        {
            p.Property(x => x.Amount)
                .HasColumnName("Value")
                .HasColumnType("decimal(24,12)")
                .HasDefaultValue(0);

            p.Property(x => x.CurrencyType)
                .HasColumnName("CurrencyType")
                .HasColumnType("tinyint")
                .HasDefaultValue(CurrencyType.None);
        });

        builder.Property(e => e.TxId)
            .HasConversion(v => v.Value, v => TxId.FromString(v))
            .HasColumnName("TxId")
            .IsRequired();

        builder.OwnsOne(x => x.Commission, c =>
        {
            c.OwnsOne(x => x.Percentage, a =>
            {
                a.Property(x => x.Amount)
                    .HasColumnName("ComPercentage")
                    .HasColumnType("decimal(24,12)")
                    .HasDefaultValue(0);

                a.Ignore(x => x.CurrencyType);
            });

            c.OwnsOne(x => x.FixedValue, a =>
            {
                a.Property(x => x.Amount)
                    .HasColumnName("ComFixedValue")
                    .HasColumnType("decimal(24,12)")
                    .HasDefaultValue(0);

                a.Property(x => x.CurrencyType)
                    .HasColumnName("ComCurrencyType")
                    .HasColumnType("tinyint")
                    .HasDefaultValue(CurrencyType.None);
            });
        });

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