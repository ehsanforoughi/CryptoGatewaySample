using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.DataAccess.AppConfigurations.CustodyAccount;

public class CustodyAccountConfig : IEntityTypeConfiguration<Domain.Entities.CustodyAccount.CustodyAccount>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.CustodyAccount.CustodyAccount> builder)
    {
        builder.ToTable(nameof(CustodyAccount), "Payment");

        builder.HasKey(x => x.CustodyAccountId);

        builder.Property(x => x.CustodyAccountId)
            .HasConversion(v => v.Value, v => new CustodyAccountId(v))
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.OwnsOne(x => x.Id)
            .Ignore(x => x.Value);

        builder.OwnsOne(x => x.CustodyAccExternalId, p =>
        {
            p.Property(x => x.Value)
                .HasMaxLength(27)
                .HasColumnName("CustodyAccExternalId");

            p.HasIndex(x => x.Value)
                .IsUnique();
        });

        builder.Property(x => x.CurrencyType)
            .HasColumnName("CurrencyType")
            .HasColumnType("tinyint")
            .IsRequired();

        builder.OwnsOne(x => x.Title)
            .Property(x => x.Value)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("Title");

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(e => e.CustomerId)
            .HasConversion(v => v.Value, v => new CustomerId(v))
            .HasColumnName("CustomerId")
            .IsRequired(false);

        builder.OwnsOne(y => y.IsActive)
            .Property(z => z.Value)
            .IsRequired()
            .HasColumnName("IsActive")
            .HasDefaultValue(false);

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