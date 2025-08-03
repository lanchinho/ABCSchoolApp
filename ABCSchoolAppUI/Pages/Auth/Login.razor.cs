using App.Infrastructure.Models;
using MudBlazor;

namespace ABCSchoolAppUI.Pages.Auth;

public partial class Login
{
    private LoginRequest _loginRequest = new();
    private InputType _inputType = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
    private bool _isPasswordVisible = false;
    private MudForm _from = default;    

    protected override async Task OnInitializedAsync()
    {
        var state = await _applicationStateProvider.GetAuthenticationStateAsync();
        if (state.User.Identity?.IsAuthenticated is true)
            _navigation.NavigateTo("/");
    }

    private async Task HandleLoginAsync()
    {   
        var result = await _tokenService
            .LoginAsync(_loginRequest.Tenant, _loginRequest);

        if (result.IsSuccessful)
        {
            _navigation.NavigateTo("/");
        }
        else
        {
            foreach (var msg in result.Messages)
            {
                _snackbar.Add(msg, Severity.Error);
            }
        }        
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

}