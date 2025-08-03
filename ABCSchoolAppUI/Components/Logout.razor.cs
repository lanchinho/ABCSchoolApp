using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace ABCSchoolAppUI.Components;
public partial class Logout
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }

    [Parameter]
    public string Title { get; set; }

    [Parameter]
    public string ConfirmationMessage { get; set; }

    [Parameter]
    public string ButtonText { get; set; }

    [Parameter]
    public Color Color { get; set; }

	private async Task OnConfirmLogoutAsync(MouseEventArgs args)
	{
        var result = await _tokenService.LogoutAsync();
        if (result.IsSuccessful)
        {
            _navigation.NavigateTo("/");
            MudDialog.Close(DialogResult.Ok(true));
        }
	}
    private void OnCancel(MouseEventArgs args)
	{
        MudDialog?.Cancel();
    }
}