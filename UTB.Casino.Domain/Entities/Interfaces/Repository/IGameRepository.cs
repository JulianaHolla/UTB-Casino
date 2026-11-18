using UTB.Casino.Domain.Enums;

namespace UTB.Casino.Domain.Entities.Interfaces.Repository;

public interface IGameRepository : IRepository<Game, int>
{
    Task<Game?> GetBySlugAsync(GameSlug slug);
    Task<bool> HasBetsAsync(int gameId);
}
