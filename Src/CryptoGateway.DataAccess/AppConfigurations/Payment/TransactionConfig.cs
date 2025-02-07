using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CryptoGateway.Domain.Entities.Transaction.ValueObjects;

namespace CryptoGateway.DataAccess.AppConfigurations.Transaction;

public class TransactionConfig : IEntityTypeConfiguration<Domain.Entities.Transaction.Transaction>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Transaction.Transaction> builder)
    {
        builder.ToTable(nameof(Transaction), "Payment");

        builder.HasKey(x => x.TransactionId);

        builder.Property(x => x.TransactionId)
            .HasConversion(v => v.Value, v => new TransactionId(v))
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.OwnsOne(x => x.Id)
            .Ignore(x => x.Value);

        builder.OwnsOne(x => x.Value, p =>
        {
            p.Property(x => x.Amount)
                .HasColumnName("Value")
                .HasColumnType("decimal(24,12)");

            p.Property(x => x.CurrencyType)
                .HasColumnName("CurrencyType")
                .HasColumnType("tinyint");
        });

        builder.OwnsOne(x => x.Balance, p =>
        {
            p.Property(x => x.Amount)
                .HasColumnName("Balance")
                .HasColumnType("decimal(24,12)");

            p.Ignore(x => x.CurrencyType);
        });

        builder.OwnsOne(x => x.ReferenceName)
            .Property(x => x.Value)
            .HasColumnName("ReferenceName")
            .HasColumnType("varchar")
            .HasMaxLength(30)
            .IsRequired();

        builder.OwnsOne(x => x.ReferenceNumber)
            .Property(x => x.Value)
            .HasColumnName("ReferenceNumber")
            .IsRequired();

        builder.Property(x => x.Type)
            .HasColumnType("tinyint")
            .IsRequired();

        builder.Property(x => x.ActionType)
            .HasColumnType("tinyint")
            .IsRequired();
        
        builder.OwnsOne(x => x.Note)
            .Property(x => x.Value)
            .HasColumnName("Note")
            .HasColumnType("nvarchar")
            .HasMaxLength(50)
            .IsRequired(false);

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