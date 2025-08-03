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

    protected override async Task OnParametersSetAsync()
    {
        var user = (await AuthState).User;
        _canViewTenants = await AuthService
              .HasPermissionAsync(user, SchoolFeature.Tenants, SchoolAction.Read);
   }
}