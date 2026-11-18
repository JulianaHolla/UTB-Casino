namespace UTB.Casino.Domain.Entities.Interfaces.Repository;

public interface IBetRepository : IRepository<Bet, int>
{
    Task<IList<Bet>> GetByUserAsync(int userId, int take);
    Task<IList<Bet>> GetRecentAsync(int take);
}
