using ABCShared.Library.Models.Requests.Users;
using ABCShared.Library.Wrappers;
using App.Infrastructure.Extensions;
using App.Infrastructure.Services.Identity;
using System.Net.Http.Json;

namespace App.Infrastructure.Services.Implementations;
public class UserService(HttpClient httpClient, ApiSettings apiSettings) : IUserService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ApiSettings _apiSettings = apiSettings;    

    public async Task<IResponseWrapper<string>> UpdateUserAsync(UpdateUserRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync(_apiSettings.UserEndpoints.Update, request);
        return await response.WrapToResponse<string>();
    }

    public async Task<IResponseWrapper<string>> ChangeUserPasswordAsync(ChangePasswordRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync(_apiSettings.UserEndpoints.ChangePassword, request);
        return await response.WrapToResponse<string>();
    }
}
