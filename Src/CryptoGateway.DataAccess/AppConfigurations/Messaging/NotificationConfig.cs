using Microsoft.EntityFrameworkCore;
using CryptoGateway.Domain.Entities.Notification;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CryptoGateway.Domain.Entities.Notification.ValueObjects;
using CryptoGateway.Domain.Entities.User.ValueObjects;

namespace CryptoGateway.DataAccess.AppConfigurations.Messaging;

public class NotificationConfig : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable(nameof(Notification), "Messaging");
        builder.HasKey(x => x.NotificationId);

        builder.Property(x => x.NotificationId)
            .HasConversion(v => v.Value, v => new NotificationId(v))
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.OwnsOne(x => x.Id)
            .Ignore(x => x.Value);

        builder.Property(x => x.UserId);

        builder.Property(x => x.Type)
            .HasColumnName("Type")
            .HasColumnType("tinyint")
            .IsRequired();

        builder.Property(x => x.ActionType)
            .HasColumnName("ActionType")
            .HasColumnType("tinyint")
            .IsRequired();

        builder.Property(x => x.PriorityType)
            .HasColumnName("PriorityType")
            .HasColumnType("tinyint")
            .IsRequired();

        builder.OwnsOne(x => x.TryCount)
            .Property(x => x.Value)
            .HasColumnName("TryCount")
            .HasDefaultValue(0)
            .IsRequired();

        builder.OwnsOne(x => x.IsSent)
            .Property(x => x.Value)
            .HasColumnName("IsSent")
            .HasDefaultValue(false)
            .IsRequired();

        builder.OwnsOne(x => x.IsSuccess)
            .Property(x => x.Value)
            .HasColumnName("IsSuccess")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.SentAt)
            .HasColumnName("SentAt")
            .IsRequired(false);

        builder.OwnsOne(x => x.SendStatus)
            .Property(x => x.Value)
            .HasColumnName("SendStatus")
            .IsRequired(false);

        builder.OwnsOne(u => u.Receiver)
            .Property(x => x.Value)
            .HasColumnName("Receiver")
            .IsRequired();

        builder.OwnsOne(x => x.Text)
            .Property(x => x.Value)
            .HasColumnName("Text")
            .IsRequired();

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