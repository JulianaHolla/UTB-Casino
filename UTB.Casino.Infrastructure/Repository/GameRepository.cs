using Microsoft.EntityFrameworkCore;
using UTB.Casino.Domain.Entities;
using UTB.Casino.Domain.Entities.Interfaces.Repository;
using UTB.Casino.Domain.Enums;
using UTB.Casino.Infrastructure.Database;

namespace UTB.Casino.Infrastructure.Repository;

public class GameRepository : Repository<Game, int>, IGameRepository
{
    public GameRepository(CasinoDbContext context) : base(context) { }

    public override async Task<IList<Game>> GetAllAsync() => await Set.OrderBy(g => g.Name).ToListAsync();
    public Task<Game?> GetBySlugAsync(GameSlug slug) => Set.FirstOrDefaultAsync(g => g.Slug == slug);
    public Task<bool> HasBetsAsync(int gameId) => Context.Bets.AnyAsync(b => b.GameId == gameId);
}
