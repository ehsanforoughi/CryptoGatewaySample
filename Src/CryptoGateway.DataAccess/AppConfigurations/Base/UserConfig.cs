using Microsoft.EntityFrameworkCore;
using CryptoGateway.Domain.Entities.User;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CryptoGateway.Domain.Entities.User.ValueObjects;

namespace CryptoGateway.DataAccess.AppConfigurations.Base;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User), "Base");

        builder.Property(x => x.Id).HasColumnName("UserId");

        builder.OwnsOne(x => x.UserExternalId, p =>
        {
            p.Property(x => x.Value)
                .HasMaxLength(27)
                .HasColumnName("UserExternalId")
                .HasConversion(v => v.ToString(), v => new UserExternalId(v));

            p.HasIndex(x => x.Value)
                .IsUnique();
        });


        //builder.Property(u => u.Email)
        //    .HasConversion(v => v.ToString(), v => new Email(v));

        builder.OwnsOne(x => x.MobileNumber, u =>
        {
            u.HasIndex(x => x.Value).IsUnique();
            u.Property(x => x.Value)
                .HasColumnName("MobileNumber")
                .HasColumnType("bigint")
                .IsRequired(false);
        });

        builder.OwnsOne(x => x.FirstName)
            .Property(x => x.Value)
            .HasColumnName("FirstName")
            .HasColumnType("nvarchar")
            .HasMaxLength(30)
            .IsRequired(false);

        builder.OwnsOne(x => x.LastName)
            .Property(x => x.Value)
            .HasColumnName("LastName")
            .HasColumnType("nvarchar")
            .HasMaxLength(30)
            .IsRequired(false);

        builder.OwnsOne(x => x.NationalCode)
            .Property(x => x.Value)
            .HasColumnName("NationalCode")
            .HasColumnType("varchar")
            .HasMaxLength(10)
            .IsRequired(false);

        builder.OwnsOne(x => x.BirthDate)
            .Property(x => x.Value)
            .HasColumnName("BirthDate")
            .IsRequired(false);

        builder.OwnsOne(x => x.Salt)
            .Property(x => x.Value)
            .HasColumnName("Salt")
            .HasColumnType("char")
            .HasMaxLength(8)
            .IsRequired(false);

        //builder.Property(u => u.Password)
        //    .HasConversion(v => v.ToString(), v => Password.FromString(v));

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.InsertDateMi)
            .HasColumnName("CreatedAt")
            .IsRequired();

        builder.Property(x => x.ModifyDateMi)
            .HasColumnName("ModifiedAt")
            .IsRequired();
    }
}