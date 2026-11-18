using UTB.Casino.Domain.Enums;

namespace UTB.Casino.Domain.Entities;

public class Game : Entity<int>
{
    public string Name { get; set; } = "";
    /// <summary>Which game it is = name of the MVC controller that plays it.</summary>
    public GameSlug Slug { get; set; }
    public string Description { get; set; } = "";
    public decimal MinBet { get; set; }
    public decimal MaxBet { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ImagePath { get; set; }

    public ICollection<Bet> Bets { get; set; } = new List<Bet>();
}
