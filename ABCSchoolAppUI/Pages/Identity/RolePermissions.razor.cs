using ABCShared.Library.Constants;
using ABCShared.Library.Models.Requests.Identity;
using ABCShared.Library.Models.Responses.Identity;
using App.Infrastructure.Constants;
using App.Infrastructure.Extensions;
using App.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Claims;

namespace ABCSchoolAppUI.Pages.Identity;
public partial class RolePermissions
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;

    [Parameter]
    public string RoleId { get; set; } = string.Empty;

    private bool _isLoading = true;
    private string _title = string.Empty;
    private string _description = string.Empty;

    private RoleResponse _roleClaimResponse = new();
    private Dictionary<string, List<RolePermissionViewModel>> RoleClaimsGroup { get; set; } = [];

    private bool _canUpdateRolePermissions;
    private string rowsPerPageString = "Rows per page:";
    private string infoFormat = "{first_item}-{last_item} of {all_items}";

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        _canUpdateRolePermissions = await AuthService.HasPermissionAsync(user, SchoolFeature.RoleClaims, SchoolAction.Update);
        await LoadRolePermissionsAsync(user);
        _isLoading = false;
    }

    private async Task LoadRolePermissionsAsync(ClaimsPrincipal user)
    {
        var result = await _roleService.GetRoleWithPermissionsAsync(RoleId);
        if (result.IsSuccessful)
        {
            _roleClaimResponse = result.Data;
            _title = "Permission management";
            _description = string.Format("Manage {0}'s Permissions", _roleClaimResponse.Name);

            var permissions = MultitenancyConstants.RootId.Equals(user.GetTenant())
                ? SchoolPermissions.All
                : SchoolPermissions.Admin;

            RoleClaimsGroup = permissions
                .GroupBy(p => p.Feature)
                .ToDictionary(g => g.Key, g => g.Select(p =>
                {
                    var permission = new RolePermissionViewModel(
                        Action: p.Action,
                        Feature: p.Feature,
                        Description: p.Description,
                        Group: p.Group,
                        isBasic: p.isBasic,
                        isRoot: p.isRoot);

                    permission.IsSelected = _roleClaimResponse.Permissions.Contains(permission.Name);
                    return permission;
                }).ToList());
        }
        else
        {
            foreach (var errorMsg in result.Messages)
            {
                _snackbar.Add(errorMsg, Severity.Error);
            }

            _navigation.NavigateTo("/roles");
        }
    }

    private async Task UpdateRolePermissionsAsync()
    {
        var allPermissions = RoleClaimsGroup.Values.SelectMany(permissions => permissions);

        var request = new UpdateRolePermissionsRequest
        {
            RoleId = RoleId,
            NewPermissions = [.. allPermissions
                .Where(rpvm => rpvm.IsSelected)
                    .Select(permission => permission.Name)]
        };
        var result = await _roleService.UpdatePermissionsAsync(request);
        if (result.IsSuccessful)
        {
            _snackbar.Add(result.Messages[0], Severity.Success);
            _navigation.NavigateTo("/roles");
        }
        else
        {
            foreach (var errorMsg in result.Messages)
            {
                _snackbar.Add(errorMsg, Severity.Error);
            }            
        }
    }

    /// <summary>
    /// Get the color of the badge base on the number of
    /// selected permissions in the group
    /// </summary>
    /// <param name="selected">total of selected permissions</param>
    /// <param name="all">all permissions</param>
    /// <returns></returns>
    private Color GetGroupBadgeColor(int selected, int all)
    {
        if (selected == 0)
            return Color.Error;
        if (selected == all)
            return Color.Success;
        return Color.Warning;
    }

    private void ReturnToRoles()
    {
        _navigation.NavigateTo("/roles");
    }
}