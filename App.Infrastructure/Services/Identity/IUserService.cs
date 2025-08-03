using ABCShared.Library.Models.Requests.Users;
using ABCShared.Library.Models.Responses.Users;
using ABCShared.Library.Wrappers;

namespace App.Infrastructure.Services.Identity;
public interface IUserService
{
    Task<IResponseWrapper<string>> UpdateUserAsync(UpdateUserRequest request);
    Task<IResponseWrapper<string>> ChangeUserPasswordAsync(ChangePasswordRequest request);
    Task<IResponseWrapper<List<UserResponse>>> GetUsersAsync();
    Task<IResponseWrapper<UserResponse>> GetByIdAsync(string userId);
}
