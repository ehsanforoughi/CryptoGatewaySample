using Microsoft.EntityFrameworkCore;
using CryptoGateway.Domain.Entities.ApiKey;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CryptoGateway.Domain.Entities.ApiKey.ValueObjects;

namespace CryptoGateway.DataAccess.AppConfigurations.Base;

public class ApiKeyConfig : IEntityTypeConfiguration<ApiKey>
{
    public void Configure(EntityTypeBuilder<ApiKey> builder)
    {
        builder.ToTable(nameof(ApiKey), "Base");
        builder.HasKey(x => x.ApiKeyId);

        builder.Property(x => x.ApiKeyId)
            .HasConversion(v => v.Value, v => new ApiKeyId(v))
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.OwnsOne(x => x.Id)
            .Ignore(x => x.Value);

        builder.Property(x => x.UserId);

        builder.Property(x => x.KeyValue)
            .HasConversion(v => v.Value, v => KeyValue.FromString(v))
            .IsRequired()
            .HasColumnName("KeyValue")
            .HasColumnType("varchar")
            .HasMaxLength(100);

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