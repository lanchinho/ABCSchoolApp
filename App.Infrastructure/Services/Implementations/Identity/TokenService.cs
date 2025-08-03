using ABCShared.Library.Models.Requests.Token;
using ABCShared.Library.Models.Responses.Token;
using ABCShared.Library.Wrappers;
using App.Infrastructure.Constants;
using App.Infrastructure.Extensions;
using App.Infrastructure.Services.Auth;
using App.Infrastructure.Services.Identity;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace App.Infrastructure.Services.Implementations.Identity;
public class TokenService : ITokenService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ApiSettings _apiSettings;

    public TokenService(
        HttpClient httpClient,
        ILocalStorageService localStorageService,
        AuthenticationStateProvider authStateProvider,
        ApiSettings apiSettings)
    {
        _httpClient = httpClient;
        _localStorageService = localStorageService;
        _authStateProvider = authStateProvider;
        _apiSettings = apiSettings;
    }

    public async Task<IResponseWrapper> LoginAsync(string tenant, TokenRequest request)
    {
        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            _apiSettings.TokenEndpoints.Login)
        {
            Content = JsonContent.Create(request)
        };
        AddTenantHeader(httpRequest, "tenant", tenant);

        var response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
        var result = await response.WrapToResponse<TokenResponse>();

        if (result.IsSuccessful)
        {
            var token = result.Data.Jwt;            
            await _localStorageService.SetItemAsync(StorageConstants.AuthToken, token);
            await _localStorageService.SetItemAsync(StorageConstants.RefreshToken, result.Data.RefreshToken);

            ((ApplicationStateProvider)_authStateProvider).MarkUserAuthenticated(request.Username);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await ResponseWrapper.SuccessAsync();
        }

        return await ResponseWrapper.FailAsync(result.Messages);           
    }

    public async Task<IResponseWrapper> LogoutAsync()
    {
        await _localStorageService.RemoveItemAsync(StorageConstants.AuthToken);
        await _localStorageService.RemoveItemAsync(StorageConstants.RefreshToken);

        ((ApplicationStateProvider)_authStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;

        return await ResponseWrapper.SuccessAsync("Successfully Logged Out");
    }

    #region Helpers
    private static void AddTenantHeader(HttpRequestMessage request, string headerName, string value)
    {
        if (string.IsNullOrWhiteSpace(headerName) || request.Headers.Contains(headerName))
            return;

        request.Headers.TryAddWithoutValidation(headerName, value);
    }
    
    #endregion
}
