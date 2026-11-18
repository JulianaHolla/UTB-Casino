namespace UTB.Casino.Domain.Entities.Interfaces.Repository;

public interface IWalletTransactionRepository : IRepository<WalletTransaction, int>
{
    Task<IList<WalletTransaction>> GetByUserAsync(int userId, int take);
    Task<IList<WalletTransaction>> GetRecentAsync(int take);
}
