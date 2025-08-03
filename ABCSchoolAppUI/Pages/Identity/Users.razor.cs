using ABCShared.Library.Models.Responses.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace ABCSchoolAppUI.Pages.Identity;
public partial class Users
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;

    private List<UserResponse> _userList = [];
    private bool _isLoading = true;
    private bool _canCreateUsers;
    private bool _canViewRoles;

    protected override async Task OnInitializedAsync()
    {
        _userList.RemoveAll(x => x.Id is not null);
        await LoadUsersAsync();
        _isLoading = false;
    }

    private async Task LoadUsersAsync() 
    {
        var result = await _userService.GetUsersAsync();
        if (result.IsSuccessful)
        {
            _userList.AddRange(result.Data);
        }
        else
        {
            foreach (var msg in result.Messages)
            {
                _snackbar.Add(msg, Severity.Warning);
            }
        }
    } 
	private void ReturnHome()
	{
        _navigation.NavigateTo("/");
	}

	private Task CreateUserAsync()
	{
		throw new NotImplementedException();
	}
}