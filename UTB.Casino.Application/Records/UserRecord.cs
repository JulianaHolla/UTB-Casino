using UTB.Casino.Domain.Entities.Interfaces;

namespace UTB.Casino.Application.Records;

/// <summary>Plain IUser used to hand user data to the account manager without knowing the Identity class.</summary>
internal record UserRecord : IUser<int>
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string DisplayName { get; set; } = "";
    public DateTime DateOfBirth { get; set; }
    public decimal Balance { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
