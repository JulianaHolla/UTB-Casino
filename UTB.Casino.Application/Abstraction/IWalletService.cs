using UTB.Casino.Application.Common;
using UTB.Casino.Domain.Entities;

namespace UTB.Casino.Application.Abstraction;

public interface IWalletService
{
    Task<decimal> GetBalanceAsync(int userId);
    Task<Result> DepositAsync(int userId, decimal amount);
    Task<Result> AdjustAsync(int userId, decimal amount, string? note);
    Task<IList<WalletTransaction>> GetUserTransactionsAsync(int userId, int take);
    Task<IList<WalletTransaction>> GetAllTransactionsAsync(int take);
    Task<WalletTransaction?> GetTransactionAsync(int id);
    Task<Result> UpdateTransactionNoteAsync(int id, string note);
}
