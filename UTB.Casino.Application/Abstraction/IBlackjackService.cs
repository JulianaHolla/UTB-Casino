using UTB.Casino.Application.Common;
using UTB.Casino.Application.ViewModels;

namespace UTB.Casino.Application.Abstraction;

public interface IBlackjackService
{
    BlackjackStateViewModel? GetState(int userId);
    Task<Result> DealAsync(int userId, decimal amount);
    Task<Result> HitAsync(int userId);
    Task<Result> StandAsync(int userId);
}
