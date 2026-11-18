using Microsoft.Extensions.Logging;
using UTB.Casino.Application.Abstraction;
using UTB.Casino.Application.Common;
using UTB.Casino.Application.GameLogic;
using UTB.Casino.Application.ViewModels;
using UTB.Casino.Domain.Entities.Interfaces.Repository;
using UTB.Casino.Domain.Enums;

namespace UTB.Casino.Application.Services;

public class SlotsService : GameServiceBase, ISlotsService
{
    private readonly ILogger<SlotsService> _logger;

    public SlotsService(IUserRepository users, IGameRepository games, IBetRepository bets,
                        IWalletTransactionRepository tx, IUnitOfWork uow, ILogger<SlotsService> logger)
        : base(users, games, bets, tx, uow) => _logger = logger;

    public async Task<Result<SlotsResultViewModel>> SpinAsync(int userId, decimal amount)
    {
        amount = Math.Round(amount, 2);
        var (user, game, error) = await PrepareAsync(userId, GameSlug.Slots, amount);
        if (error != null) return Result<SlotsResultViewModel>.Fail(error);

        var spin = SlotMachine.Spin();
        var payout = Math.Round(amount * spin.Multiplier, 2);
        await SettleAsync(user!, game!, amount, payout, spin.Description, false);

        _logger.LogInformation("Slots: user {UserId} bet {Amount}, payout {Payout}", userId, amount, payout);
        return Result<SlotsResultViewModel>.Ok(new SlotsResultViewModel(spin.Reels, payout, spin.Description));
    }
}
