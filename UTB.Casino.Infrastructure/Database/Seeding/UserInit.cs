using UTB.Casino.Infrastructure.Identity;

namespace UTB.Casino.Infrastructure.Database.Seeding;

/// <summary>
/// Initial accounts. Only password HASHES are stored here (never the passwords themselves);
/// the passwords are handed over outside of the source code and must be changed after the first login.
/// </summary>
internal static class UserInit
{
    private static readonly DateTime Created = new(2026, 11, 1, 0, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime BirthDate = new(1990, 1, 1);

    public static User GetAdmin() => new()
    {
        Id = 1,
        UserName = "admin",
        NormalizedUserName = "ADMIN",
        Email = "admin@casino.local",
        NormalizedEmail = "ADMIN@CASINO.LOCAL",
        EmailConfirmed = true,
        DisplayName = "Administrator",
        DateOfBirth = BirthDate,
        CreatedAt = Created,
        Balance = 0m,
        IsBlocked = false,
        PasswordHash = "AQAAAAEAAYagAAAAEOptWUKPaPstlzSUxZEj3V+pyyxkQ6FNHQVbpQVyK9dZsixNW4D0mmFMLAurbMh2ug==",
        SecurityStamp = "K3GQXW5NCV4UJ2R7M6ZPTD8YHBL9EA1F",
        ConcurrencyStamp = "5d0c3c0e-6a49-4c6b-9d52-0b7f4a1e8c11",
        LockoutEnabled = true
    };

    public static User GetManager() => new()
    {
        Id = 2,
        UserName = "manager",
        NormalizedUserName = "MANAGER",
        Email = "manager@casino.local",
        NormalizedEmail = "MANAGER@CASINO.LOCAL",
        EmailConfirmed = true,
        DisplayName = "Manager",
        DateOfBirth = BirthDate,
        CreatedAt = Created,
        Balance = 0m,
        IsBlocked = false,
        PasswordHash = "AQAAAAEAAYagAAAAEAmOaHI+ChEOY/NUvyfelrUG2qWYcvSTbcaQF4i2GMlcPKO9nuVJ7GaCucDusCvsAg==",
        SecurityStamp = "T7NPA2LJ5XQ9WE4RB8MCHY3UD6KZGV1S",
        ConcurrencyStamp = "8a2f6b71-3e0d-4d5a-b1c9-7e4d2f9a6b22",
        LockoutEnabled = true
    };
}
