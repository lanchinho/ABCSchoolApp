using ABCSchoolAppUI.Components;
using MudBlazor;

namespace ABCSchoolAppUI.Layout;
public partial class MainLayout
{
    private bool _drawerOpen = true;

    override protected void OnInitialized()
    {
        _interceptor.RegisterEvent();
        StateHasChanged();
    }

    private void ToggleDrawer()
        => _drawerOpen = !_drawerOpen;

    private async Task LogOut()
    {
        var parameters = new DialogParameters
        {
            { nameof(Logout.Title), nameof(Logout) },
            { nameof(Logout.ConfirmationMessage), "Are you sure you want to logout?" },
            { nameof(Logout.ButtonText), "Sign Out" },
            { nameof(Logout.Color), Color.Success }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            BackdropClick = true
        };

        await _dialogService.ShowAsync<Logout>(title: null, parameters: parameters, options: options);
    }
}