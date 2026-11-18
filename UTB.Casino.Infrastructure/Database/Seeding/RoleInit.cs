using UTB.Casino.Domain.Enums;
using UTB.Casino.Infrastructure.Identity;

namespace UTB.Casino.Infrastructure.Database.Seeding;

internal static class RoleInit
{
    public static List<Role> GetRoles() => new()
    {
        Create(Roles.Admin, "9cf14c2c-19e7-40d6-b744-8917505c3106"),
        Create(Roles.Manager, "be0efcde-9d0a-461d-8eb6-444b043d6660"),
        Create(Roles.Customer, "29dafca7-cd20-4cd9-a3dd-4779d7bac3ee")
    };

    private static Role Create(Roles role, string concurrencyStamp) => new()
    {
        Id = (int)role,
        Name = role.ToString(),
        NormalizedName = role.ToString().ToUpperInvariant(),
        ConcurrencyStamp = concurrencyStamp
    };
}
