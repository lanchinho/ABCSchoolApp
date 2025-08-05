using ABCShared.Library.Constants;

namespace App.Infrastructure.Models;
public record RolePermissionViewModel : SchoolPermission
{
    public RolePermissionViewModel(
        string Action,
        string Feature,
        string Description,
        string Group = "",
        bool isBasic = false,
        bool isRoot = false) 
        : base(Action, Feature, Description, Group, isBasic, isRoot)
    {
    }
    public bool IsSelected { get; set; }
}
