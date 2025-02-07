using Bat.Core;

namespace CryptoGateway.Domain.Entities.Auth;

public class Menu : IEntity
{
    public int MenuId { get; set; }

    public Menu Parent { get; set; }
    public int? ParentId { get; set; }

    public byte OrderPriority { get; set; }

    public bool ShowInMenu { get; set; } = true;

    public string Icon { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }


    public List<Menu> ChildMenus { get; set; }
    public List<MenuInRole> MenuInRoles { get; set; }
}