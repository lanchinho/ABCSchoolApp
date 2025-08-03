using ABCShared.Library.Constants;
using Microsoft.AspNetCore.Authorization;

namespace App.Infrastructure.Services.Auth;
internal class ShouldHavePermissionAttribute : AuthorizeAttribute
{
    public ShouldHavePermissionAttribute(string action, string feature)
    {
        Policy = SchoolPermission.NameFor(action, feature);
    }
}
