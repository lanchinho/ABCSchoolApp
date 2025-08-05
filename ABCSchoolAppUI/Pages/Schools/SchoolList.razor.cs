using ABCSchoolAppUI.Pages.Tenancy;
using ABCShared.Library.Constants;
using ABCShared.Library.Models.Responses.Schools;
using App.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace ABCSchoolAppUI.Pages.Schools;
public partial class SchoolList
{
    public List<SchoolResponse> Schools { get; set; } = [];

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;

    [CascadingParameter]
    private Task<AuthenticationState> AuthState { get; set; } = default!;

    private bool _isLoading = true;
    private bool _canCreateSchool = false;
    private bool _canUpdateSchool = false;
    private bool _canDeleteSchool = false;
    private string rowsPerPageString = "Rows per page:";
    private string infoFormat = "{first_item}-{last_item} of {all_items}";

    protected override async Task OnInitializedAsync()

    {
        var user = (await AuthState).User;
        _canCreateSchool = await AuthService.HasPermissionAsync(user, SchoolFeature.Schools, SchoolAction.Create);
        _canUpdateSchool = await AuthService.HasPermissionAsync(user, SchoolFeature.Schools, SchoolAction.Update);
        _canDeleteSchool = await AuthService.HasPermissionAsync(user, SchoolFeature.Schools, SchoolAction.Delete);

        await LoadSchoolsAsync();
        _isLoading = false;
    }

    private async Task LoadSchoolsAsync()
    {
        Schools.RemoveAll(x => x.Id > 0);

        var result = await _schoolsService.GetAllAsync();
        if (result.IsSuccessful)
        {
            Schools = result.Data;
        }
        else
        {
            foreach (var errorMsg in result.Messages)
            {
                _snackbar.Add(errorMsg, Severity.Error);
            }
        }
    }

    private async Task OnBoardNewSchoolAsync()
    {
        var parameters = new DialogParameters<CreateSchool>();
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
        var dialog = await _dialogService.ShowAsync<CreateSchool>("Register new School", parameters, options);

        //resultado da operação na modal, se sucesso: fecha modal e atualiza tabela
        var result = await dialog.Result;
        if (!result.Canceled)
            await LoadSchoolsAsync();
    }


    private void ReturnHome()
    {
        _navigation.NavigateTo("/");
    }
}