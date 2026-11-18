using UTB.Casino.Application.Common;
using UTB.Casino.Domain.Entities;

namespace UTB.Casino.Application.Abstraction;

public interface IBetService
{
    Task<IList<Bet>> GetUserBetsAsync(int userId, int take);
    Task<IList<Bet>> GetAllAsync(int take);
    Task<Bet?> GetByIdAsync(int id);
    Task<Result> UpdateResultAsync(int id, string result);
    Task<Result> DeleteAsync(int id);
}
