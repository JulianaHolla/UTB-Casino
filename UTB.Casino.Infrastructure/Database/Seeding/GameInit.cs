using UTB.Casino.Domain.Enums;

namespace UTB.Casino.Infrastructure.Database.Seeding;

internal static class GameInit
{
    /// <summary>Anonymous objects: seed data must not set navigation properties (Game.Bets).</summary>
    public static object[] GetGames() => new object[]
    {
        new { Id = 1, Name = "Lucky Reels", Slug = GameSlug.Slots, MinBet = 1m, MaxBet = 100m, IsActive = true,
              Description = "Three reels, one line. Triple sevens pay 100 times your bet." },
        new { Id = 2, Name = "European Roulette", Slug = GameSlug.Roulette, MinBet = 1m, MaxBet = 500m, IsActive = true,
              Description = "Numbers 0 to 36. Back a number, a colour, odd or even, or a half." },
        new { Id = 3, Name = "Blackjack", Slug = GameSlug.Blackjack, MinBet = 5m, MaxBet = 250m, IsActive = true,
              Description = "Beat the dealer to 21. Blackjack pays 3 to 2, dealer stands on 17." }
    };
}
