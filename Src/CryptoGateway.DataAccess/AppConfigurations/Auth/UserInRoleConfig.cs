using Microsoft.EntityFrameworkCore;
using CryptoGateway.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoGateway.DataAccess.AppConfigurations.Auth;

public class UserInRoleConfig : IEntityTypeConfiguration<UserInRole>
{
    public void Configure(EntityTypeBuilder<UserInRole> builder)
    {
        builder.ToTable(nameof(UserInRole), "Auth");

        builder.Property(x => x.UserInRoleId)
            .UseIdentityColumn();
    }
}