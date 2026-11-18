using UTB.Casino.Domain.Entities.Interfaces;

namespace UTB.Casino.Domain.Entities;

public class WalletTransaction : Entity<int>
{
    public int UserId { get; set; }
    public int? BetId { get; set; }
    public int TransactionTypeId { get; set; }
    //Signed: negative = credits out, positive = credits in
    public decimal Amount { get; set; }
    public string Note { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public IUser<int>? User { get; set; }
    public Bet? Bet { get; set; }
    public TransactionType? TransactionType { get; set; }
}
