using App.Infrastructure.Services.Identity;
using App.Infrastructure.Services.Interceptors;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Headers;
using Toolbelt.Blazor;

namespace App.Infrastructure.Services.Implementations.Interceptors;
public class HttpRefreshTokenInterceptor(HttpClientInterceptor httpClientInterceptor, ITokenService tokenService, NavigationManager navigation, ISnackbar snackbar) : IHttpRefreshTokenInterceptor
{
    private readonly HttpClientInterceptor _httpClientInterceptor = httpClientInterceptor;
    private readonly ITokenService _tokenService = tokenService;
    private readonly NavigationManager _navigation = navigation;
    private readonly ISnackbar _snackbar = snackbar;

    public async Task InterceptBeforeHttpRequestAsync(object sender, HttpClientInterceptorEventArgs eventArgs)
    {
        var absPath = eventArgs.Request.RequestUri.AbsolutePath;
        if (!absPath.Contains("token"))
        {
            try
            {
                var newlyRefreshedJwt = await _tokenService.TryForceRefreshTokenAsync();
                if(!string.IsNullOrWhiteSpace(newlyRefreshedJwt))                
                    eventArgs.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newlyRefreshedJwt);            
            }
            catch (Exception)
            {
                _snackbar.Add("You are Logget Out.", Severity.Error);
                await _tokenService.LogoutAsync();
                _navigation.NavigateTo("/");
            }
        }           
    }

    public void RegisterEvent() 
        => _httpClientInterceptor.BeforeSendAsync += InterceptBeforeHttpRequestAsync;
    
}
