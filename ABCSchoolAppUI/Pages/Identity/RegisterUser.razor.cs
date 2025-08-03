using ABCShared.Library.Models.Requests.Users;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ABCSchoolAppUI.Pages.Identity;
public partial class RegisterUser
{
    private CreateUserRequest CreateUserRequest { get; set; } = new();
    [CascadingParameter] private IMudDialogInstance Dialog { get; set; }
    private InputType _inputType = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
    private bool _isPasswordVisible;

    private InputType _confirmPasswordInputType = InputType.Password;
    private string _confirmPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
    private bool _isconfirmPasswordVisible;

    private MudForm _form;

    private async Task SubmitUserRegistrationAsync()
    {
        var result = await _userService.RegisterUserAsync(CreateUserRequest);
        if (result.IsSuccessful)
        {
            _snackbar.Add(result.Messages[0], Severity.Success);
            Dialog.Close(DialogResult.Ok(true));
        }
        else
        {
            foreach (var error in result.Messages)
            {
                _snackbar.Add(error, Severity.Error);
            }
        }
    }

    private void CancelDialog()
    {
        Dialog.Close();
    }

    private void TogglePasswordVisibility()
    {
        if (_isPasswordVisible)
        {
            _isPasswordVisible = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _inputType = InputType.Password;
        }
        else
        {
            _isPasswordVisible = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _inputType = InputType.Text;
        }
    }

    private void ToggleConfirmPasswordVisibility()
    {
        if (_isconfirmPasswordVisible)
        {
            _isconfirmPasswordVisible = false;
            _confirmPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            _confirmPasswordInputType = InputType.Password;
        }
        else
        {
            _isconfirmPasswordVisible = true;
            _confirmPasswordInputIcon = Icons.Material.Filled.Visibility;
            _confirmPasswordInputType = InputType.Text;
        }
    }
}