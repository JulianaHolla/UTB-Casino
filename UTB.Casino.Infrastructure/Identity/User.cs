using Microsoft.AspNetCore.Identity;
using UTB.Casino.Domain.Entities.Interfaces;

namespace UTB.Casino.Infrastructure.Identity;

/// <summary>Our Identity user. Implements the domain IUser, so Domain and Application never see Identity.</summary>
public class User : IdentityUser<int>, IUser<int>
{
    public string DisplayName { get; set; } = "";
    public DateTime DateOfBirth { get; set; }
    public decimal Balance { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
