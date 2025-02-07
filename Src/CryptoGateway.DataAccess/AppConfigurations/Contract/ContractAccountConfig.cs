using Microsoft.EntityFrameworkCore;
using CryptoGateway.Domain.Entities.ContractAccount;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CryptoGateway.Domain.Entities.ContractAccount.ValueObjects;

namespace CryptoGateway.DataAccess.AppConfigurations.Contract;

public class ContractAccountConfig : IEntityTypeConfiguration<ContractAccount>
{
    public void Configure(EntityTypeBuilder<ContractAccount> builder)
    {
        builder.ToTable(nameof(ContractAccount), "Contract");

        builder.HasKey(x => x.ContractAccountId);

        builder.Property(x => x.ContractAccountId)
            .HasConversion(v => v.Value, v => new ContractAccountId(v))
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.OwnsOne(x => x.Id)
            .Ignore(x => x.Value);

        builder.OwnsOne(x => x.Address, p =>
        {
            p.Property(x => x.Base58Value)
                .HasColumnName("AddressBase58")
                .IsRequired();

            p.Property(x => x.HexValue)
                .HasColumnName("AddressHex")
                .IsRequired();
        });

        builder.OwnsOne(y => y.PrivateKey)
            .Property(z => z.Value)
            .IsRequired()
            .HasColumnName("PrivateKey");

        builder.OwnsOne(y => y.PublicKey)
            .Property(z => z.Value)
            .IsRequired()
            .HasColumnName("PublicKey");

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