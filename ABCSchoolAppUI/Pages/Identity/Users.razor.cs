using ABCSchoolAppUI.Pages.Tenancy;
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

    private async Task InvokeUserRegistrationDialogAsync()
    {
        var options = new DialogOptions
        {
            CloseButton = true,
            CloseOnEscapeKey = true,
            BackdropClick = false,
            FullWidth = true,
            MaxWidth = MaxWidth.Small,
            Position = DialogPosition.Center,
        };

        var dialog = await _dialogService.ShowAsync<RegisterUser>(title: null, options: options);
        var result = await dialog.Result;
        if (!result.Canceled)
            await LoadUsersAsync();
    }

    private void ReturnHome()
    {
        _navigation.NavigateTo("/");
    }
}