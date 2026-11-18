using UTB.Casino.Application.Common;
using UTB.Casino.Application.ViewModels;

namespace UTB.Casino.Application.Abstraction;

public interface IRouletteService
{
    Task<Result<RouletteResultViewModel>> SpinAsync(int userId, decimal amount, string betType, int? number);
}
