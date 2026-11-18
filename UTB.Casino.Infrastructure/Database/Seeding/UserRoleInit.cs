using Microsoft.AspNetCore.Identity;
using UTB.Casino.Domain.Enums;

namespace UTB.Casino.Infrastructure.Database.Seeding;

internal static class UserRoleInit
{
    public static List<IdentityUserRole<int>> GetUserRoles() => new()
    {
        new() { UserId = 1, RoleId = (int)Roles.Admin },
        new() { UserId = 2, RoleId = (int)Roles.Manager }
    };
}
