using ABCShared.Library.Constants;
using App.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace ABCSchoolAppUI.Layout;
public partial class NavMenu
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;

    private bool _canViewTenants;
    private bool _canViewUsers;

    protected override async Task OnParametersSetAsync()
    {
        var user = (await AuthState).User;
        _canViewTenants = await AuthService
              .HasPermissionAsync(user, SchoolFeature.Tenants, SchoolAction.Read);

        _canViewUsers = await AuthService
              .HasPermissionAsync(user, SchoolFeature.Users, SchoolAction.Read);
    }
}