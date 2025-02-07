using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Payment.ValueObjects;

namespace CryptoGateway.DataAccess.AppConfigurations.Payment;

public class PaymentConfig : IEntityTypeConfiguration<Domain.Entities.Payment.Payment>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Payment.Payment> builder)
    {
        builder.ToTable(nameof(Payment), "Payment");

        builder.HasKey(x => x.PaymentId);

        builder.Property(x => x.PaymentId)
            .HasConversion(v => v.Value, v => new PaymentId(v))
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.OwnsOne(x => x.Id)
            .Ignore(x => x.Value);

        builder.OwnsOne(x => x.PaymentExternalId, p =>
        {
            p.Property(x => x.Value)
                .HasMaxLength(27)
                .HasColumnName("PaymentExternalId");

            p.HasIndex(x => x.Value)
                .IsUnique();
        });

        builder.Property(e => e.UserId)
            .IsRequired();

        builder.OwnsOne(x => x.Price, p =>
        {
            p.Property(x => x.Amount)
                .HasColumnName("PriceAmount")
                .HasColumnType("decimal(24,12)")
                .HasDefaultValue(0);

            p.Property(x => x.CurrencyType)
                .HasColumnName("PriceCurrencyType")
                .HasColumnType("tinyint")
                .HasDefaultValue(CurrencyType.None);
        });

        builder.OwnsOne(x => x.Pay, p =>
        {
            p.Property(x => x.Amount)
                .HasColumnName("PayAmount")
                .HasColumnType("decimal(24,12)")
                .HasDefaultValue(0);

            p.Property(x => x.CurrencyType)
                .HasColumnName("PayCurrencyType")
                .HasColumnType("tinyint")
                .HasDefaultValue(CurrencyType.None);
        });

        builder.OwnsOne(x => x.SpotUsdtPrice, p =>
        {
            p.Property(x => x.Amount)
                .HasColumnName("SpotUsdtPrice")
                .HasColumnType("decimal(24,12)")
                .HasDefaultValue(0);

            p.Ignore(x => x.CurrencyType);
        });
        
        builder.OwnsOne(x => x.CallBackUrl)
            .Property(x => x.Value)
            .IsRequired(false)
            .HasColumnName("CallBackUrl");

        builder.Property(e => e.CustomerId)
            .HasConversion(v => v.Value, v => new CustomerId(v))
            .HasColumnName("CustomerId")
            .IsRequired(false);

        builder.Property(e => e.CustomerContact)
            .HasConversion(v => v.Value, v => new CustomerContact(v))
            .HasColumnName("CustomerContact")
            .IsRequired(false);

        builder.Property(x => x.OrderId)
            .HasConversion(v => v.Value, v => new OrderId(v))
            .HasColumnName("OrderId")
            .IsRequired(false);

        builder.OwnsOne(x => x.OrderDescription)
            .Property(x => x.Value)
            .IsRequired(false)
            .HasColumnName("OrderDesc");

        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.InsertDateMi)
            .HasColumnName("CreatedAt")
            .IsRequired();

        builder.Property(x => x.ModifyDateMi)
            .HasColumnName("ModifiedAt")
            .IsRequired();

        builder.OwnsOne(x => x.ExpiredAt)
            .Property(x => x.Value)
            .HasColumnName("ExpiredAt")
            .IsRequired();
    }
}