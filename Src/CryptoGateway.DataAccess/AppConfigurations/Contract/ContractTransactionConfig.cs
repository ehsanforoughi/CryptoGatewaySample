using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CryptoGateway.Domain.Entities.ContractTransaction;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.ContractTransaction.ValueObjects;

namespace CryptoGateway.DataAccess.AppConfigurations.Contract;

public class ContractTransactionConfig : IEntityTypeConfiguration<ContractTransaction>
{
    public void Configure(EntityTypeBuilder<ContractTransaction> builder)
    {
        builder.ToTable(nameof(ContractTransaction), "Contract");

        builder.HasKey(x => x.ContractTransactionId);

        builder.Property(x => x.ContractTransactionId)
            .HasConversion(v => v.Value, v => new ContractTransactionId(v))
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.OwnsOne(x => x.Id)
            .Ignore(x => x.Value);

        builder.Property(e => e.TxId)
            .HasConversion(v => v.Value, v => TxId.FromString(v))
            .HasColumnName("TxId")
            .IsRequired();

        builder.Property(x => x.Timestamp).IsRequired(false);
        builder.Property(x => x.Symbol).IsRequired(false);
        builder.Property(x => x.ContractType).IsRequired(false);
        builder.Property(x => x.ContractResource).IsRequired(false);
        builder.Property(x => x.ContractData).IsRequired(false);
        builder.Property(x => x.ContractAddress).IsRequired(false);
        builder.Property(x => x.OwnerAddress).IsRequired(false);
        builder.Property(x => x.ReceiverAddress).IsRequired(false);
        builder.Property(x => x.FromAddress).IsRequired(false);
        builder.Property(x => x.ToAddress).IsRequired(false);
        builder.Property(x => x.Amount).IsRequired(false).HasColumnType("decimal(24,12)");
        builder.Property(x => x.Expiration).IsRequired(false);
        builder.Property(x => x.RefBlockBytes).IsRequired(false);
        builder.Property(x => x.RefBlockHash).IsRequired(false);
        builder.Property(x => x.FeeLimit).IsRequired(false).HasColumnType("decimal(24,12)");
        builder.Property(x => x.Signature).IsRequired(false);
        builder.Property(x => x.EnergyUsage).IsRequired(false);
        builder.Property(x => x.EnergyFee).IsRequired(false).HasColumnType("decimal(24,12)");
        builder.Property(x => x.GasLimit).IsRequired(false);
        builder.Property(x => x.GasPrice).IsRequired(false).HasColumnType("decimal(24,12)");
        builder.Property(x => x.BandwidthUsage).IsRequired(false);
        builder.Property(x => x.BandwidthFee).IsRequired(false).HasColumnType("decimal(24,12)");

        builder.Property(x => x.InsertDateMi)
            .HasColumnName("CreatedAt")
            .IsRequired();

        builder.Property(x => x.ModifyDateMi)
            .HasColumnName("ModifiedAt")
            .IsRequired();
    }
}