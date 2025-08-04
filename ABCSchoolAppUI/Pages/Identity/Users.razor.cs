using ABCSchoolAppUI.Components;
using ABCShared.Library.Models.Requests.Users;
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

    public List<UserResponse> UserList { get; set; } = [];
    private bool _isLoading = true;
    private bool _canCreateUsers;
    private bool _canViewRoles;

    protected override async Task OnInitializedAsync()
    {
        await LoadUsersAsync();
        _isLoading = false;
    }

    private async Task LoadUsersAsync()
    {
        UserList.RemoveAll(x => x.Id is not null);

        var result = await _userService.GetUsersAsync();
        if (result.IsSuccessful)
        {
            UserList.AddRange(result.Data);
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

    private async Task ChangeStatusAsync(UserResponse user)
    {

        var statusModificationMsg = user.IsActive
                       ? $"Are you sure you want to Deactivate user {user.FirstName}{user.LastName}"
                       : $"Are you sure you want to Activaste user {user.FirstName}{user.LastName}";
        var parameters = new DialogParameters
            {
                { nameof(Confirmation.Title), user.IsActive? "Deactivate User" : "Activate User"},
                { nameof(Confirmation.Message), statusModificationMsg},
                { nameof(Confirmation.ButtonText), user.IsActive? "Deactivate" : "Activate"},
                { nameof(Confirmation.Color), Color.Primary},
                { nameof(Confirmation.InputIcon), Icons.Material.Filled.Person}
            };

        var options = new DialogOptions
        {
            CloseButton = true,
            CloseOnEscapeKey = true,
            BackdropClick = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        var dialog = await _dialogService.ShowAsync<Confirmation>(title: null, parameters, options);
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            var response = await _userService.ChangeUserStatusAsync(new ChangeUserStatusRequest
            {
                UserId = user.Id,
                Activation = !user.IsActive
            });

            if (response.IsSuccessful)
            {
                _snackbar.Add(response.Messages[0], Severity.Success);
                await LoadUsersAsync();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackbar.Add(message, Severity.Error);
                }
            }
        }
    }

    private void GoToRoles(string userId)
    {
        _navigation.NavigateTo($"/user-roles/{userId}");
    }

    private void ReturnHome()
    {
        _navigation.NavigateTo("/");
    }
}