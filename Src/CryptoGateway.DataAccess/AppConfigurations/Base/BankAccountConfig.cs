using Microsoft.EntityFrameworkCore;
using CryptoGateway.Domain.Entities.BankAccount;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

namespace CryptoGateway.DataAccess.AppConfigurations.Base;

public class BankAccountConfig : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.ToTable(nameof(BankAccount), "Base");

        builder.HasKey(x => x.BankAccountId);

        builder.Property(x => x.BankAccountId)
            .HasConversion(v => v.Value, v => new BankAccountId(v))
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.OwnsOne(x => x.Id)
            .Ignore(x => x.Value);

        builder.OwnsOne(x => x.BankAccountTitle)
            .Property(x => x.Value)
            .IsRequired()
            .HasColumnName("Title")
            .HasColumnType("nvarchar")
            .HasMaxLength(50);

        builder.Property(x => x.State)
            .IsRequired()
            .HasColumnName("State")
            .HasColumnType("tinyint");

        builder.Property(x => x.BankType)
            .IsRequired()
            .HasColumnName("Type")
            .HasColumnType("tinyint");

        builder.Property(x => x.CardNumber)
            .HasConversion(v => v.Value, v => CardNumber.FromString(v))
            .IsRequired()
            .HasColumnName("CardNumber")
            .HasColumnType("varchar")
            .HasMaxLength(19);

        builder.OwnsOne(x => x.Sheba)
            .Property(x => x.Value)
            .IsRequired()
            .HasColumnName("Sheba")
            .HasColumnType("varchar")
            .HasMaxLength(26);

        builder.OwnsOne(x => x.AccountNumber)
            .Property(x => x.Value)
            .IsRequired(false)
            .HasColumnName("AccountNumber")
            .HasColumnType("varchar")
            .HasMaxLength(18);

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