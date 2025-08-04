using ABCShared.Library.Constants;
using ABCShared.Library.Models.Responses.Identity;
using App.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace ABCSchoolAppUI.Pages.Identity;

public partial class Roles
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;

    [Inject]
    protected IAuthorizationService Authservice { get; set; } = default!;

    public List<RoleResponse> RoleList { get; set; } = [];

    private bool _isLoading = true;    
    private bool _canCreateRole = false;
    private bool _canUpdateRole = false;
    private bool _canDeleteRole = false;
    private bool _canViewRolePermissions = false; //Permissão dos roles são os Claims...
    private string rowsPerPageString = "Rows per page:";
    private string infoFormat = "{first_item}-{last_item} of {all_items}";

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canCreateRole = await Authservice.HasPermissionAsync(user, SchoolFeature.Roles, SchoolAction.Create);
        _canUpdateRole = await Authservice.HasPermissionAsync(user, SchoolFeature.Roles, SchoolAction.Update);
        _canDeleteRole = await Authservice.HasPermissionAsync(user, SchoolFeature.Roles, SchoolAction.Delete);
        _canViewRolePermissions = await Authservice.HasPermissionAsync(user, SchoolFeature.RoleClaims, SchoolAction.Read);

        await LoadRolesAsync();
        _isLoading = false;
    }

    private async Task LoadRolesAsync()
    {
        RoleList.RemoveAll(r => r.Id is not null);

        var result = await _roleService.GetRolesAsync();
        if (result.IsSuccessful)
        {
            RoleList = result.Data;
        }
        else
        {
            foreach(var errorMsg in result.Messages)
            {
                _snackbar.Add(errorMsg, Severity.Error);
            }
        }
    }

    private void ReturnHome()
    {
        _navigation.NavigateTo("/");
    }
}