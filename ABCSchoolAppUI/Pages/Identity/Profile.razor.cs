using ABCShared.Library.Models.Requests.Users;
using App.Infrastructure.Extensions;
using MudBlazor;

namespace ABCSchoolAppUI.Pages.Identity;
public partial class Profile
{
    private UpdateUserRequest UpdateUserRequest { get; set; } = new();
    private string FirstName { get; set; }
    private string LastName { get; set; }
    private char UserInitial { get; set; }
    private string Email { get; set; }
    public string UserId { get; set; }

    private MudForm _form;

    protected override async Task OnInitializedAsync()
    {
        await SetCurrentUserDetails();
    }

    private async Task SetCurrentUserDetails()
    {
        var state = await _applicationStateProvider.GetAuthenticationStateAsync();
        var user = state.User;
        UpdateUserRequest.Id = UserId = user.GetUserId();
        UpdateUserRequest.FirstName = FirstName = user.GetFirstName();
        UpdateUserRequest.LastName = LastName = user.GetLastName();
        UpdateUserRequest.PhoneNumber = user.GetPhoneNumber();
        Email = user.GetEmail();
        

        if (FirstName.Length > 0)
            UserInitial = FirstName[0];        
    }

    private async Task UpdateUserDetailsAsync()
    {
        var result = await _userService.UpdateUserAsync(UpdateUserRequest);
        if (result.IsSuccessful)
        {
            await _tokenService.LogoutAsync();
            _snackbar.Add("Your profile has been updated successfully. Login again.", Severity.Success);
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
