using ABCShared.Library.Models.Requests.Tenancy;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ABCSchoolAppUI.Pages.Tenancy;
public partial class UpgradeSubscription
{
    [CascadingParameter]
    private IMudDialogInstance DialogInstance { get; set; }
    private MudForm _form;

    [Parameter]
    public UpdateTenantSubscriptionRequest SubscriptionRequest { get; set; } = new();

    private DateTime? NewExpiryDatePicker
    {
        get => SubscriptionRequest.NewExpiryDate == default
            ? null
            : SubscriptionRequest.NewExpiryDate;

        set 
        {
            if (value.HasValue)
                SubscriptionRequest.NewExpiryDate = value.Value;
        }
    }

	private async Task UpgradeSubscriptionAsync()
	{
        var result = await _tenantService.UpgradeSubscriptionAsync(SubscriptionRequest);
        if (result.IsSuccessful)
        {
            _snackbar.Add(result.Messages[0], Severity.Success);
            DialogInstance.Close(DialogResult.Ok(true));
        }
        else
        {
            foreach (var error in result.Messages)
            {
                _snackbar.Add(error, Severity.Error);
            }
        }
    }
	
	private void CancelDialog(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
	{
        DialogInstance.Close();
    }
}