using Microsoft.AspNetCore.Components;

namespace ABCSchoolAppUI.Components;
public partial class NotAuthorized
{
    [Parameter] public string Message { get; set; }

    private void GoHome()
    {
        _navigation.NavigateTo("/");
    }
}