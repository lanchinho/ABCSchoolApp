using ABCShared.Library.Models.Requests.Users;
using App.Infrastructure.Extensions;
using MudBlazor;

namespace ABCSchoolAppUI.Pages.Identity;

public partial class PasswordReset
{
    private ChangePasswordRequest ChangePasswordRequest { get; set; } = new();

    private bool _currentPasswordVisibility;
    private InputType _currentPasswordInput = InputType.Password;
    private string _currentPasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    private bool _newPasswordVisibility;
    private InputType _newPasswordInput = InputType.Password;
    private string _newPasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    private MudForm _form;

    protected override async Task OnInitializedAsync()
    {
        await SetCurrentUserDetails();
    }

    private async Task SetCurrentUserDetails()
    {
        var state = await _applicationStateProvider.GetAuthenticationStateAsync();
        ChangePasswordRequest.UserId = state.User.GetUserId();
    }

    private void TogglePasswordVisibility(bool isNewPassword)
    {
        if (isNewPassword)
        {
            _newPasswordVisibility = !_newPasswordVisibility;
            _newPasswordInput = _newPasswordVisibility ? InputType.Text : InputType.Password;
            _currentPasswordInputIcon = _newPasswordVisibility ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;
        }
        else
        {
            _currentPasswordVisibility = !_currentPasswordVisibility;
            _currentPasswordInput = _currentPasswordVisibility ? InputType.Text : InputType.Password;
            _newPasswordInputIcon = _currentPasswordVisibility ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;
        }
    }
    private async Task ResetPasswordAsync()
    {
        var result = await _userService.ChangeUserPasswordAsync(ChangePasswordRequest);
        if (result.IsSuccessful)
        {
            await _tokenService.LogoutAsync();
            _snackbar.Add("Your password has been changed. Please login again.", Severity.Success);
            _navigation.NavigateTo("/");
        }
        else
        {
            foreach (var error in result.Messages)
            {
                _snackbar.Add(error, Severity.Error);
            }
        }
    }
}