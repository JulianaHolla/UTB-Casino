using Microsoft.EntityFrameworkCore;
using UTB.Casino.Domain.Entities;
using UTB.Casino.Domain.Entities.Interfaces.Repository;
using UTB.Casino.Infrastructure.Database;

namespace UTB.Casino.Infrastructure.Repository;

public class WalletTransactionRepository : Repository<WalletTransaction, int>, IWalletTransactionRepository
{
    public WalletTransactionRepository(CasinoDbContext context) : base(context) { }

    public override Task<WalletTransaction?> GetByIdAsync(int id) =>
        Set.Include(t => t.User).Include(t => t.TransactionType).FirstOrDefaultAsync(t => t.Id == id);

    public override async Task<IList<WalletTransaction>> GetAllAsync() => await GetRecentAsync(int.MaxValue);

    public async Task<IList<WalletTransaction>> GetByUserAsync(int userId, int take) =>
        await Set.Include(t => t.TransactionType).Where(t => t.UserId == userId).OrderByDescending(t => t.CreatedAt).Take(take).ToListAsync();

    public async Task<IList<WalletTransaction>> GetRecentAsync(int take) =>
        await Set.Include(t => t.User).Include(t => t.TransactionType).OrderByDescending(t => t.CreatedAt).Take(take).ToListAsync();
}
