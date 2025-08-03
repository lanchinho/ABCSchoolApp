using ABCShared.Library.Constants;
using App.Infrastructure.Constants;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace App.Infrastructure.Services.Auth;
public class ApplicationStateProvider(
    ILocalStorageService localStorageService,
    HttpClient httpClient) : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService = localStorageService;
    private readonly HttpClient _httpClient = httpClient;

    public ClaimsPrincipal AuthenticationStateUser { get; set; }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        //recupera token JWT da local storag do navegador...
        var savedToken = await _localStorageService.GetItemAsync<string>(StorageConstants.AuthToken);
        if (string.IsNullOrWhiteSpace(savedToken))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

        var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(GetClaimsFromJwt(savedToken), "jwt")));
        AuthenticationStateUser = state.User;

        return state;
    }

    public void MarkUserAuthenticated()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public async Task<ClaimsPrincipal> GetAuthenticationStateProviderUserAsync()
    {
        var state = await GetAuthenticationStateAsync();
        return state.User;
    }

    #region Helpers
    private static IEnumerable<Claim> GetClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split(".")[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);

        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        if (keyValuePairs != null)
        {
            TranslateArrayOfClaimTypeToClaimTypeKeyValue(claims, keyValuePairs, ClaimTypes.Role);
            TranslateArrayOfClaimTypeToClaimTypeKeyValue(claims, keyValuePairs, ClaimConstants.Permission);


            //keyValuePairs.TryGetValue(ClaimConstants.Permission, out var permissions);
            //         if (permissions != null)
            //         {
            //             if (permissions.ToString().Trim().StartsWith('['))
            //             {
            //                 var parsedPermissions = JsonSerializer.Deserialize<string[]>(permissions.ToString());
            //                 claims.AddRange(parsedPermissions.Select(permission => new Claim(ClaimConstants.Permission, permission)));
            //             }
            //             else
            //             {
            //                 claims.Add(new Claim(ClaimConstants.Permission, permissions.ToString()));
            //             }
            //             keyValuePairs.Remove(ClaimConstants.Permission);
            //         }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
        }
        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64Payload)
    {
        switch (base64Payload.Length % 4)
        {
            case 2:
                base64Payload += "==";
                break;
            case 3:
                base64Payload += "=";
                break;
        }

        return Convert.FromBase64String(base64Payload);
    }

    private static void TranslateArrayOfClaimTypeToClaimTypeKeyValue(List<Claim> claims, Dictionary<string, object> keyValuePairs, string claimType)
    {
        keyValuePairs.TryGetValue(claimType, out var claimTypeValue);
        if (claimTypeValue != null)
        {
            if (claimTypeValue.ToString().Trim().StartsWith('['))
            {
                var parsedType = JsonSerializer.Deserialize<string[]>(claimTypeValue.ToString());
                claims.AddRange(parsedType.Select(typeName => new Claim(claimType, typeName)));
            }
            else
            {
                claims.Add(new Claim(claimType, claimTypeValue.ToString()));
            }
            keyValuePairs.Remove(claimType);
        }
    }

    #endregion
}
