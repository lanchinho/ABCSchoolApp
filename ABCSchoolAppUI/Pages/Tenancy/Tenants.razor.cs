using ABCSchoolAppUI.Components;
using ABCShared.Library.Models.Requests.Tenancy;
using ABCShared.Library.Models.Responses.Tenancy;
using MudBlazor;

namespace ABCSchoolAppUI.Pages.Tenancy;
public partial class Tenants
{
    public List<TenantResponse> TenantList { get; set; } = [];
    private bool _isLoading = true;
    private string rowsPerPageString = "Rows per page:";
    private string infoFormat = "{first_item}-{last_item} of {all_items}";

    protected async override Task OnInitializedAsync()
    {
        await LoadTenantsAsync();
        _isLoading = false;
    }

    private async Task LoadTenantsAsync()
    {
        TenantList.RemoveAll(x => x.Identifier is not null);
        var result = await _tenantService.GetAllAsync();
        if (result.IsSuccessful)
        {
            TenantList.AddRange(result.Data);
        }
        else
        {
            foreach (var error in result.Messages)
            {
                _snackbar.Add(error, Severity.Error);
            }
        }
    }

    private void ReturnClicked()
    {
        _navigation.NavigateTo("/");
    }

    private async Task OnBoardNewTenantAsync()
    {
        var parameters = new DialogParameters<CreateTenant>();
        //opções da modal
        var options = new DialogOptions
        {
            CloseButton = true,
            CloseOnEscapeKey = true,
            BackdropClick = false,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            Position = DialogPosition.Center,
        };

        //faz a modal aparecer
        var dialog = await _dialogService.ShowAsync<CreateTenant>("Onboard New Tenant", parameters, options);

        //resultado da operação na modal, se sucesso: fecha modal e atualiza tabela
        var result = await dialog.Result;
        if (!result.Canceled)
            await LoadTenantsAsync();
    }

    private async Task UpgradeSubscriptionAsync(TenantResponse tentant)
    {

        var parameters = new DialogParameters
        {
            {
              nameof(UpgradeSubscription.SubscriptionRequest),
              new UpdateTenantSubscriptionRequest
              {
                  TenantId = tentant.Identifier,
                  NewExpiryDate =  tentant.ValidUpTo
              }
            }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            CloseOnEscapeKey = true,
            BackdropClick = false,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            Position = DialogPosition.Center,
        };
        var dialog = await _dialogService.ShowAsync<UpgradeSubscription>("Upgrade Tenant Subscription", parameters, options);

        //resultado da operação na modal, se sucesso: fecha modal e atualiza tabela
        var result = await dialog.Result;
        if (!result.Canceled)
            await LoadTenantsAsync();
    }

    private async Task ChangeStatusAsync(TenantResponse tenant)
    {

        var statusModificationMsg = tenant.IsActive ? $"Are you sure you want to Deactivate tenant {tenant.Name}" : $"Are you sure you want to Activaste tenant {tenant.Name}";
        var parameters = new DialogParameters
            {
                { nameof(Confirmation.Title), tenant.IsActive? "Deactivate Tenant" : "Activate Tenant"},
                { nameof(Confirmation.Message), statusModificationMsg},
                { nameof(Confirmation.ButtonText), tenant.IsActive? "Deactivate" : "Activate"},
                { nameof(Confirmation.Color), tenant.IsActive? Color.Success : Color.Primary},
                { nameof(Confirmation.InputIcon), tenant.IsActive? Icons.Material.Filled.CloudOff : Icons.Material.Filled.CloudDone}
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
            var response = tenant.IsActive
                           ? await _tenantService.DeActivateAsync(tenant.Identifier)
                           : await _tenantService.ActivateAsync(tenant.Identifier);

            if (response.IsSuccessful)
            {
                _snackbar.Add(response.Messages[0], Severity.Success);
                await LoadTenantsAsync();
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
}