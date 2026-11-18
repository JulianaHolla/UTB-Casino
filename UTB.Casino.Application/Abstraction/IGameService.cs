using UTB.Casino.Application.Common;
using UTB.Casino.Application.ViewModels;
using UTB.Casino.Domain.Entities;
using UTB.Casino.Domain.Enums;

namespace UTB.Casino.Application.Abstraction;

public interface IGameService
{
    Task<IList<Game>> GetAllAsync();
    Task<IList<Game>> GetActiveAsync();
    Task<Game?> GetByIdAsync(int id);
    Task<Game?> GetBySlugAsync(GameSlug slug);
    Task<Result> CreateAsync(GameEditViewModel vm, Stream? image, string? imageFileName);
    Task<Result> UpdateAsync(GameEditViewModel vm, Stream? image, string? imageFileName);
    Task<Result> DeleteAsync(int id);
}
