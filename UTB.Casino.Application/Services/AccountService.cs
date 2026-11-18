using Microsoft.Extensions.Logging;
using UTB.Casino.Application.Abstraction;
using UTB.Casino.Application.Common;
using UTB.Casino.Application.Records;
using UTB.Casino.Application.Validations;
using UTB.Casino.Application.ViewModels;
using UTB.Casino.Domain.Entities.Interfaces.Managers;
using UTB.Casino.Domain.Entities.Interfaces.Repository;
using UTB.Casino.Domain.Enums;

namespace UTB.Casino.Application.Services;

public class AccountService : IAccountService
{
    private const int MinimumAge = 18;
    private readonly IAccountManager<int> _accountManager;
    private readonly IUserRepository _users;
    private readonly ILogger<AccountService> _logger;

    public AccountService(IAccountManager<int> accountManager, IUserRepository users, ILogger<AccountService> logger)
    { _accountManager = accountManager; _users = users; _logger = logger; }

    public async Task<Result> RegisterAsync(RegisterViewModel vm)
    {
        if (vm.DateOfBirth is not { } dateOfBirth || !AgeCalculator.IsAtLeast(dateOfBirth, MinimumAge))
            return Result.Fail($"You must be at least {MinimumAge} years old.");

        var user = new UserRecord
        {
            UserName = vm.UserName, Email = vm.Email, DisplayName = vm.DisplayName,
            DateOfBirth = dateOfBirth, CreatedAt = DateTime.UtcNow
        };
        var errors = await _accountManager.CreateAsync(user, vm.Password, Roles.Customer);
        if (errors.Length > 0) return Result.Fail(errors);

        _logger.LogInformation("User {UserName} registered", vm.UserName);
        return Result.Ok();
    }

    public Task<LoginResult> LoginAsync(LoginViewModel vm) =>
        _accountManager.LoginAsync(vm.UserName, vm.Password, vm.RememberMe);

    public Task LogoutAsync() => _accountManager.LogoutAsync();

    public async Task<IList<UserViewModel>> GetUsersAsync()
    {
        var list = new List<UserViewModel>();
        foreach (var u in await _users.GetAllAsync())
            list.Add(new UserViewModel
            {
                Id = u.Id, UserName = u.UserName ?? "", Email = u.Email ?? "", DisplayName = u.DisplayName,
                Balance = u.Balance, IsBlocked = u.IsBlocked, CreatedAt = u.CreatedAt,
                Role = await _accountManager.GetRoleAsync(u.Id) ?? Roles.Customer
            });
        return list;
    }

    public async Task<UserEditViewModel?> GetUserAsync(int id)
    {
        var user = await _users.GetByIdAsync(id);
        if (user == null) return null;
        return new UserEditViewModel
        {
            Id = user.Id, UserName = user.UserName ?? "", Email = user.Email ?? "", DisplayName = user.DisplayName,
            IsBlocked = user.IsBlocked, Balance = user.Balance,
            Role = await _accountManager.GetRoleAsync(user.Id) ?? Roles.Customer
        };
    }

    public async Task<Result> UpdateUserAsync(UserEditViewModel vm)
    {
        var current = await _users.GetByIdAsync(vm.Id);
        if (current == null) return Result.Fail("User not found.");
        if (!Enum.IsDefined(vm.Role)) return Result.Fail("Unknown role.");

        var currentRole = await _accountManager.GetRoleAsync(vm.Id);
        if (currentRole == Roles.Admin && vm.Role != Roles.Admin && await _accountManager.CountInRoleAsync(Roles.Admin) <= 1)
            return Result.Fail("The last administrator cannot lose the Admin role.");

        var errors = await _accountManager.UpdateAsync(
            new UserRecord { Id = vm.Id, UserName = vm.UserName, Email = vm.Email, DisplayName = vm.DisplayName, IsBlocked = vm.IsBlocked },
            vm.Role);
        if (errors.Length > 0) return Result.Fail(errors);

        _logger.LogInformation("User {UserId} updated by admin", vm.Id);
        return Result.Ok();
    }
}
