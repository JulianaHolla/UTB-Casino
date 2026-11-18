using Microsoft.Extensions.Logging;
using UTB.Casino.Application.Abstraction;
using UTB.Casino.Application.Common;
using UTB.Casino.Application.ViewModels;
using UTB.Casino.Domain.Entities;
using UTB.Casino.Domain.Entities.Interfaces.Repository;
using UTB.Casino.Domain.Entities.Interfaces.Storage;
using UTB.Casino.Domain.Enums;

namespace UTB.Casino.Application.Services;

public class GameService : IGameService
{
    private static readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
    private const long MaxImageBytes = 2 * 1024 * 1024;

    private readonly IGameRepository _games;
    private readonly IUnitOfWork _uow;
    private readonly IFileStorage _files;
    private readonly ILogger<GameService> _logger;

    public GameService(IGameRepository games, IUnitOfWork uow, IFileStorage files, ILogger<GameService> logger)
    { _games = games; _uow = uow; _files = files; _logger = logger; }

    public Task<IList<Game>> GetAllAsync() => _games.GetAllAsync();
    public async Task<IList<Game>> GetActiveAsync() => (await _games.GetAllAsync()).Where(g => g.IsActive).ToList();
    public Task<Game?> GetByIdAsync(int id) => _games.GetByIdAsync(id);
    public Task<Game?> GetBySlugAsync(GameSlug slug) => _games.GetBySlugAsync(slug);

    public async Task<Result> CreateAsync(GameEditViewModel vm, Stream? image, string? imageFileName)
    {
        var error = Validate(vm);
        if (error != null) return Result.Fail(error);
        if (!Enum.IsDefined(vm.Slug)) return Result.Fail("Unknown game type.");
        if (await _games.GetBySlugAsync(vm.Slug) != null) return Result.Fail("A game of this type already exists.");

        var (path, imageError) = await StoreImageAsync(image, imageFileName);
        if (imageError != null) return Result.Fail(imageError);

        await _games.AddAsync(new Game
        {
            Name = vm.Name.Trim(), Slug = vm.Slug, Description = vm.Description?.Trim() ?? "",
            MinBet = vm.MinBet, MaxBet = vm.MaxBet, IsActive = vm.IsActive, ImagePath = path
        });
        await _uow.SaveChangesAsync();
        _logger.LogInformation("Game {Slug} created", vm.Slug);
        return Result.Ok();
    }

    public async Task<Result> UpdateAsync(GameEditViewModel vm, Stream? image, string? imageFileName)
    {
        var game = await _games.GetByIdAsync(vm.Id);
        if (game == null) return Result.Fail("Game not found.");
        var error = Validate(vm);
        if (error != null) return Result.Fail(error);

        var (path, imageError) = await StoreImageAsync(image, imageFileName);
        if (imageError != null) return Result.Fail(imageError);
        if (path != null) { _files.Delete(game.ImagePath); game.ImagePath = path; }

        game.Name = vm.Name.Trim();
        game.Description = vm.Description?.Trim() ?? "";
        game.MinBet = vm.MinBet;
        game.MaxBet = vm.MaxBet;
        game.IsActive = vm.IsActive;
        _games.Update(game);
        await _uow.SaveChangesAsync();
        _logger.LogInformation("Game {GameId} updated", game.Id);
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var game = await _games.GetByIdAsync(id);
        if (game == null) return Result.Fail("Game not found.");
        if (await _games.HasBetsAsync(id)) return Result.Fail("This game has bets and cannot be deleted. Close it instead.");

        _files.Delete(game.ImagePath);
        _games.Remove(game);
        await _uow.SaveChangesAsync();
        _logger.LogInformation("Game {GameId} deleted", id);
        return Result.Ok();
    }

    private static string? Validate(GameEditViewModel vm)
    {
        if (string.IsNullOrWhiteSpace(vm.Name)) return "Name is required.";
        if (vm.MinBet <= 0 || vm.MaxBet < vm.MinBet) return "Min bet must be above 0 and not higher than max bet.";
        return null;
    }

    private async Task<(string? Path, string? Error)> StoreImageAsync(Stream? image, string? fileName)
    {
        if (image == null) return (null, null);
        var ext = Path.GetExtension(fileName ?? "").ToLowerInvariant();
        if (!ImageExtensions.Contains(ext)) return (null, "Image must be .jpg, .png, .webp or .gif.");
        if (image.Length > MaxImageBytes) return (null, "Image must be smaller than 2 MB.");
        return (await _files.SaveImageAsync(image, ext), null);
    }
}
