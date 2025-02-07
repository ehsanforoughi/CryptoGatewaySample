using Bat.Core;

namespace CryptoGateway.Domain.Entities.Auth;

public class MenuInRole : IEntity
{
    public int MenuInRoleId { get; set; }

    public Role Role { get; set; }
    public int RoleId { get; set; }

    public Menu Menu { get; set; }
    public int MenuId { get; set; }

    public bool IsDefault { get; set; }
}