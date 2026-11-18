using UTB.Casino.Domain.Entities.Interfaces;

namespace UTB.Casino.Domain.Entities;

public class Bet : Entity<int>
{
    public int UserId { get; set; }
    public int GameId { get; set; }
    public decimal Amount { get; set; }
    //Total returned to the player (stake included). 0 = lost
    public decimal Payout { get; set; }
    public string Result { get; set; } = "";
    public DateTime PlacedAt { get; set; } = DateTime.UtcNow;

    public IUser<int>? User { get; set; }
    public Game? Game { get; set; }
    public ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
}
