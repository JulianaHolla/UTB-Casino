using UTB.Casino.Application.Common;
using UTB.Casino.Application.ViewModels;

namespace UTB.Casino.Application.Abstraction;

public interface ISlotsService
{
    Task<Result<SlotsResultViewModel>> SpinAsync(int userId, decimal amount);
}
