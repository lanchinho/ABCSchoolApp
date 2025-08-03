using ABCShared.Library.Models.Requests.Tenancy;
using ABCShared.Library.Models.Responses.Tenancy;
using ABCShared.Library.Wrappers;
using App.Infrastructure.Extensions;
using App.Infrastructure.Services.Tenancy;
using System.Net.Http.Json;

namespace App.Infrastructure.Services.Implementations.Tenancy;

public class TenantService(HttpClient httpClient, ApiSettings apiSettings) : ITenantService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ApiSettings _apiSettings = apiSettings;

    public async Task<IResponseWrapper<string>> ActivateAsync(string tenantId)
    {
        var response = await _httpClient.PutAsJsonAsync(_apiSettings.TenantEndpoints.GetActivateEndpointUrl(tenantId),tenantId);
        return await response.WrapToResponse<string>();
    }

    public async Task<IResponseWrapper<string>> CreateAsync(CreateTenantRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(_apiSettings.TenantEndpoints.Create, request);
        return await response.WrapToResponse<string>();
    }

    public async Task<IResponseWrapper<string>> DeActivateAsync(string tenantId)
    {
        var response = await _httpClient.PutAsJsonAsync(_apiSettings.TenantEndpoints.GetDeActivateEndpointUrl(tenantId), tenantId);
        return await response.WrapToResponse<string>();
    }

    public async Task<IResponseWrapper<List<TenantResponse>>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync(_apiSettings.TenantEndpoints.All);
        return await response.WrapToResponse<List<TenantResponse>>();
    }

    public async Task<IResponseWrapper<TenantResponse>> GetByIdAsync(string tenantId)
    {
        var response = await _httpClient.GetAsync(_apiSettings.TenantEndpoints.GetByIdEndpointUrl(tenantId));
        return await response.WrapToResponse<TenantResponse>();
    }

    public async Task<IResponseWrapper<string>> UpgradeSubscriptionAsync(UpdateTenantSubscriptionRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync(_apiSettings.TenantEndpoints.Upgrade, request);
        return await response.WrapToResponse<string>();
    }
}
