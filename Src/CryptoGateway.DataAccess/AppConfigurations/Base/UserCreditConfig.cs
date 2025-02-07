using Microsoft.EntityFrameworkCore;
using CryptoGateway.Domain.Entities.UserCredit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CryptoGateway.Domain.Entities.UserCredit.ValueObjects;

namespace CryptoGateway.DataAccess.AppConfigurations.Base;

public class UserCreditConfig : IEntityTypeConfiguration<UserCredit>
{
    public void Configure(EntityTypeBuilder<UserCredit> builder)
    {
        builder.ToTable(nameof(UserCredit), "Base");

        builder.HasKey(x => x.UserCreditId);

        builder.Property(x => x.UserCreditId)
            .HasConversion(v => v.Value, v => new UserCreditId(v))
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.OwnsOne(x => x.Id)
            .Ignore(x => x.Value);

        builder.Property(x => x.UserId)
            .IsRequired();

        //builder.Property(e => e.UserId)
        //    .HasConversion(v => v.Value, v => new UserId(v))
        //    .IsRequired();

        builder.OwnsOne(x => x.RealCredit, p =>
        {
            p.Property(x => x.Amount)
                .HasColumnName("Value")
                .HasColumnType("decimal(24,12)");

            p.Property(x => x.CurrencyType)
                .HasColumnName("CurrencyType")
                .HasColumnType("tinyint");
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