using ABCShared.Library.Models.Requests.Identity;
using ABCShared.Library.Models.Responses.Identity;
using ABCShared.Library.Wrappers;
using App.Infrastructure.Services.Identity;
using System.Net.Http;

namespace App.Infrastructure.Services.Implementations.Identity;
public class RoleService(HttpClient httpClient, ApiSettings apiSettings) : IRoleService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ApiSettings _apiSettings = apiSettings;

    public Task<IResponseWrapper<string>> CreateAsync(CreateRoleRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<IResponseWrapper<string>> DeleteAsync(string roleId)
    {
        throw new NotImplementedException();
    }

    public Task<IResponseWrapper<List<RoleResponse>>> GetRolesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IResponseWrapper<RoleResponse>> GetRoleWithoutPermissions(string roleId)
    {
        throw new NotImplementedException();
    }

    public Task<IResponseWrapper<RoleResponse>> GetRoleWithPermissions(string roleId)
    {
        throw new NotImplementedException();
    }

    public Task<IResponseWrapper<string>> UpdateAsync(UpdateRoleRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<IResponseWrapper<string>> UpdatePermissionsAsync(UpdateRolePermissionsRequest request)
    {
        throw new NotImplementedException();
    }
}
