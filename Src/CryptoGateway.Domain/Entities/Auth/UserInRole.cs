using Bat.Core;

namespace CryptoGateway.Domain.Entities.Auth;

public class UserInRole : IEntity
{
    public int UserInRoleId { get; set; }

    public Role Role { get; set; }
    public int RoleId { get; set; }

    //public User.User User { get; set; }
    public int UserId { get; set; }
}