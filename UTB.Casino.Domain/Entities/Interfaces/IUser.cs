namespace UTB.Casino.Domain.Entities.Interfaces;

/// <summary>Player as the domain sees it. The Identity-based implementation lives in Infrastructure.</summary>
public interface IUser<TKey> : IEntity<TKey> where TKey : notnull
{
    string? UserName { get; set; }
    string? Email { get; set; }
    string DisplayName { get; set; }
    DateTime DateOfBirth { get; set; }
    decimal Balance { get; set; }
    bool IsBlocked { get; set; }
    DateTime CreatedAt { get; set; }
}
