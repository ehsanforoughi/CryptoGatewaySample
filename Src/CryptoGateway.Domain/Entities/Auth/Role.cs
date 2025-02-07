using Bat.Core;

namespace CryptoGateway.Domain.Entities.Auth;

public class Role : IEntity, IInsertDateProperty
{
    public int RoleId { get; set; }

    public RoleType Type { get; set; }

    public bool IsActive { get; set; }

    public string RoleNameFa { get; set; }

    public string RoleNameEn { get; set; }

    public DateTime InsertDateMi { get; set; }


    public List<MenuInRole> MenuInRoles { get; set; }
    public List<UserInRole> UserInRoles { get; set; }
}