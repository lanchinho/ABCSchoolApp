using ABCShared.Library.Models.Requests.Schools;
using ABCShared.Library.Models.Responses.Schools;
using ABCShared.Library.Wrappers;
using App.Infrastructure.Extensions;
using App.Infrastructure.Services.Schools;
using System.Net.Http.Json;

namespace App.Infrastructure.Services.Implementations;
public class SchoolsService(HttpClient httpClient, ApiSettings apiSettings) : ISchoolsService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ApiSettings _apiSettings = apiSettings;

    public async Task<IResponseWrapper<int>> CreateSchoolAsync(CreateSchoolRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(_apiSettings.SchoolEndpoints.Create, request);
        return await response.WrapToResponseForStructs<int>();
    }

    public async Task<IResponseWrapper<int>> DeleAsync(string schoolId)
    {
        var response = await _httpClient.DeleteAsync(_apiSettings.SchoolEndpoints.GetDeleteEndpoint(schoolId));
        return await response.WrapToResponseForStructs<int>();
    }

    public async Task<IResponseWrapper<List<SchoolResponse>>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync(_apiSettings.SchoolEndpoints.All);
        return await response.WrapToResponse<List<SchoolResponse>>();
    }

    public async Task<IResponseWrapper<SchoolResponse>> GetByIdAsync(string schoolId)
    {
        var response = await _httpClient.GetAsync(_apiSettings.SchoolEndpoints.GetByIdEndpoint(schoolId));
        return await response.WrapToResponse<SchoolResponse>();
    }

    public async Task<IResponseWrapper<SchoolResponse>> GetByNameAsync(string schoolName)
    {
        var response = await _httpClient.GetAsync(_apiSettings.SchoolEndpoints.GetByNameEndpoint(schoolName));
        return await response.WrapToResponse<SchoolResponse>();
    }

    public async Task<IResponseWrapper<int>> UpdateAsync(UpdateSchoolRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync(_apiSettings.SchoolEndpoints.Update, request);
        return await response.WrapToResponseForStructs<int>();
    }
}
