using Microsoft.AspNetCore.Identity;
using UTB.Casino.Domain.Entities.Interfaces;
using UTB.Casino.Domain.Entities.Interfaces.Managers;
using UTB.Casino.Domain.Enums;
using UTB.Casino.Infrastructure.Identity;

namespace UTB.Casino.Infrastructure.Managers;

/// <summary>The only place that uses UserManager and SignInManager.</summary>
public class AccountIdentityManager : IAccountManager<int>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountIdentityManager(UserManager<User> userManager, SignInManager<User> signInManager)
    { _userManager = userManager; _signInManager = signInManager; }

    public async Task<string[]> CreateAsync(IUser<int> data, string password, params Roles[] roles)
    {
        var user = new User
        {
            UserName = data.UserName, Email = data.Email, DisplayName = data.DisplayName,
            DateOfBirth = data.DateOfBirth, CreatedAt = data.CreatedAt
        };

        var created = await _userManager.CreateAsync(user, password);
        if (!created.Succeeded) return Describe(created);

        var errors = new List<string>();
        foreach (var role in roles)
        {
            var added = await _userManager.AddToRoleAsync(user, role.ToString());
            if (!added.Succeeded) errors.AddRange(Describe(added));
        }
        return errors.ToArray();
    }

    public async Task<LoginResult> LoginAsync(string userName, string password, bool rememberMe)
    {
        var result = await _signInManager.PasswordSignInAsync(userName, password, rememberMe, lockoutOnFailure: false);
        if (result.Succeeded) return LoginResult.Succeeded;
        return result.IsNotAllowed ? LoginResult.NotAllowed : LoginResult.InvalidCredentials;
    }

    public Task LogoutAsync() => _signInManager.SignOutAsync();

    public async Task<Roles?> GetRoleAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return null;
        var names = await _userManager.GetRolesAsync(user);
        return names.Select(n => Enum.TryParse<Roles>(n, out var role) ? (Roles?)role : null).FirstOrDefault(r => r != null);
    }

    public async Task<int> CountInRoleAsync(Roles role) =>
        (await _userManager.GetUsersInRoleAsync(role.ToString())).Count;

    public async Task<string[]> UpdateAsync(IUser<int> data, Roles role)
    {
        var user = await _userManager.FindByIdAsync(data.Id.ToString());
        if (user == null) return new[] { "User not found." };

        // Only profile fields are touched - the password hash is never read or written here.
        var errors = new List<string>();
        if (user.UserName != data.UserName) errors.AddRange(Describe(await _userManager.SetUserNameAsync(user, data.UserName)));
        if (user.Email != data.Email) errors.AddRange(Describe(await _userManager.SetEmailAsync(user, data.Email)));
        if (errors.Count > 0) return errors.ToArray();

        user.DisplayName = data.DisplayName;
        user.IsBlocked = data.IsBlocked;
        var updated = await _userManager.UpdateAsync(user);
        if (!updated.Succeeded) return Describe(updated);

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Count != 1 || currentRoles[0] != role.ToString())
        {
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, role.ToString());
        }
        return Array.Empty<string>();
    }

    private static string[] Describe(IdentityResult result) => result.Errors.Select(e => e.Description).ToArray();
}
