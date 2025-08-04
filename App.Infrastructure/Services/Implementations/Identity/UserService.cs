using ABCShared.Library.Models.Requests.Users;
using ABCShared.Library.Models.Responses.Users;
using ABCShared.Library.Wrappers;
using App.Infrastructure.Extensions;
using App.Infrastructure.Services.Identity;
using System.Net.Http.Json;

namespace App.Infrastructure.Services.Implementations.Identity;
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

    public async Task<IResponseWrapper<List<UserResponse>>> GetUsersAsync()
    {
        var response = await _httpClient.GetAsync(_apiSettings.UserEndpoints.All);
        return await response.WrapToResponse<List<UserResponse>>();
    }

    public async Task<IResponseWrapper<UserResponse>> GetByIdAsync(string userId)
    {
        var response = await _httpClient.GetAsync(_apiSettings.UserEndpoints.UserByIdEndpoint(userId));
        return await response.WrapToResponse<UserResponse>();
    }

    public async Task<IResponseWrapper<string>> RegisterUserAsync(CreateUserRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(_apiSettings.UserEndpoints.Register, request);
        return await response.WrapToResponse<string>();
    }

    public async Task<IResponseWrapper<List<UserRoleResponse>>> GetUserRolesAsync(string userId)
    {
        var response = await _httpClient.GetAsync(_apiSettings.UserEndpoints.RolesByIdEndpoint(userId));
        return await response.WrapToResponse<List<UserRoleResponse>>();
    }

    public async Task<IResponseWrapper<string>> UpdateUserRolesAsync(string userId, UserRolesRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync(_apiSettings.UserEndpoints.UpdateRolesEndpoint(userId), request);
        return await response.WrapToResponse<string>();
    }

    public async Task<IResponseWrapper<string>> ChangeUserStatusAsync(ChangeUserStatusRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync(_apiSettings.UserEndpoints.UpdateStatus, request);
        return await response.WrapToResponse<string>();
    }
}
