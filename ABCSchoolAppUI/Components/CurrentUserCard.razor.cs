using App.Infrastructure.Extensions;

namespace ABCSchoolAppUI.Components;

public partial class CurrentUserCard
{
    private string FirstName { get; set; }
    private string LastName { get; set; }
    private char UserNameInitial { get; set; }
    private string Email { get; set; }
    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        await SetCurrentUserDetails();
        _isLoading = false;
    }

    private async Task SetCurrentUserDetails()
    {
        var state = await _applicationStateProvider.GetAuthenticationStateAsync();

        var user = state.User;
        FirstName = user.GetFirstName();
        LastName = user.GetLastName();
        Email = user.GetEmail();
        UserNameInitial = FirstName.Length > 0 ? FirstName[0] : default;
    }
}