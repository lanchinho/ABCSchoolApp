using ABCShared.Library.Models.Requests.Schools;
using ABCShared.Library.Models.Responses.Schools;
using ABCShared.Library.Wrappers;

namespace App.Infrastructure.Services.Schools;
public interface ISchoolsService
{
    Task<IResponseWrapper<SchoolResponse>> GetByIdAsync(string schoolId);    
    Task<IResponseWrapper<SchoolResponse>> GetByNameAsync(string schoolName);
    Task<IResponseWrapper<List<SchoolResponse>>> GetAllAsync();
    Task<IResponseWrapper<int>> CreateSchoolAsync(CreateSchoolRequest request);
    Task<IResponseWrapper<int>> UpdateAsync(UpdateSchoolRequest request);
    Task<IResponseWrapper<int>> DeleAsync(string schoolId);
}
