using UTB.Casino.Application.Common;
using UTB.Casino.Application.ViewModels;
using UTB.Casino.Domain.Enums;

namespace UTB.Casino.Application.Abstraction;

public interface IAccountService
{
    Task<Result> RegisterAsync(RegisterViewModel vm);
    Task<LoginResult> LoginAsync(LoginViewModel vm);
    Task LogoutAsync();
    Task<IList<UserViewModel>> GetUsersAsync();
    Task<UserEditViewModel?> GetUserAsync(int id);
    Task<Result> UpdateUserAsync(UserEditViewModel vm);
}
