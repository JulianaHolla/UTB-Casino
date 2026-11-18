using UTB.Casino.Domain.Enums;

namespace UTB.Casino.Domain.Entities.Interfaces.Managers;

/// <summary>Account operations (users, passwords, roles, sign-in). Implemented in Infrastructure on top of ASP.NET Core Identity.</summary>
public interface IAccountManager<TKey> where TKey : notnull
{
    /// <summary>Creates the user. Returns error messages, empty when it succeeded.</summary>
    Task<string[]> CreateAsync(IUser<TKey> user, string password, params Roles[] roles);

    Task<LoginResult> LoginAsync(string userName, string password, bool rememberMe);
    Task LogoutAsync();

    Task<Roles?> GetRoleAsync(TKey userId);
    Task<int> CountInRoleAsync(Roles role);

    /// <summary>Updates user name, e-mail, display name, blocked flag and the role. Never touches the password.</summary>
    Task<string[]> UpdateAsync(IUser<TKey> user, Roles role);
}
