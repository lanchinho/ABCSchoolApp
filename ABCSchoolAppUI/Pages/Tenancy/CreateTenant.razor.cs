using ABCShared.Library.Models.Requests.Tenancy;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ABCSchoolAppUI.Pages.Tenancy;
public partial class CreateTenant
{
    [CascadingParameter]
    private IMudDialogInstance DialogInstance { get; set; }

    private CreateTenantRequest CreateTenantRequest { get; set; } = new();
    private MudForm _form;
    private MudDatePicker _datePicker = default!;
    private DateTime? ValidUpToPicker
    {
        get => CreateTenantRequest.ValidUpTo == default
            ? null
            : CreateTenantRequest.ValidUpTo;

        set
        {
            if (value.HasValue)
                CreateTenantRequest.ValidUpTo = value.Value;
        }
    }

    private void CancelDialog()
    {
        DialogInstance.Close();
    }

    private async Task SaveTenantAsync()
    {
        var result = await _tenantService.CreateAsync(CreateTenantRequest);
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
}