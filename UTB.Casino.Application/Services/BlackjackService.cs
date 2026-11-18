using Microsoft.Extensions.Logging;
using UTB.Casino.Application.Abstraction;
using UTB.Casino.Application.Common;
using UTB.Casino.Application.GameLogic;
using UTB.Casino.Application.ViewModels;
using UTB.Casino.Domain.Entities;
using UTB.Casino.Domain.Entities.Interfaces;
using UTB.Casino.Domain.Entities.Interfaces.Repository;
using UTB.Casino.Domain.Enums;

namespace UTB.Casino.Application.Services;

public class BlackjackService : GameServiceBase, IBlackjackService
{
    private readonly IBlackjackRoundStore _store;
    private readonly ILogger<BlackjackService> _logger;

    public BlackjackService(IUserRepository users, IGameRepository games, IBetRepository bets,
                            IWalletTransactionRepository tx, IUnitOfWork uow,
                            IBlackjackRoundStore store, ILogger<BlackjackService> logger)
        : base(users, games, bets, tx, uow) { _store = store; _logger = logger; }

    public BlackjackStateViewModel? GetState(int userId)
    {
        var round = _store.Get(userId);
        return round == null ? null : ToViewModel(round);
    }

    public async Task<Result> DealAsync(int userId, decimal amount)
    {
        if (_store.Get(userId) is { IsFinished: false }) return Result.Fail("Finish the current hand first.");

        amount = Math.Round(amount, 2);
        var (user, game, error) = await PrepareAsync(userId, GameSlug.Blackjack, amount);
        if (error != null) return Result.Fail(error);

        user!.Balance -= amount;
        await Transactions.AddAsync(new WalletTransaction
        {
            UserId = user.Id, Amount = -amount, TransactionTypeId = (int)TransactionTypes.Stake, Note = game!.Name
        });

        var round = BlackjackEngine.Start(amount);
        _store.Set(userId, round);

        if (round.IsFinished) await FinishAsync(user, game, round);
        else await Uow.SaveChangesAsync();

        _logger.LogInformation("Blackjack: user {UserId} dealt a hand for {Amount}", userId, amount);
        return Result.Ok();
    }

    public async Task<Result> HitAsync(int userId)
    {
        if (_store.Get(userId) is not { IsFinished: false } round) return Result.Fail("No hand in progress.");
        BlackjackEngine.Hit(round);
        if (round.IsFinished) await FinishByIdAsync(userId, round);
        return Result.Ok();
    }

    public async Task<Result> StandAsync(int userId)
    {
        if (_store.Get(userId) is not { IsFinished: false } round) return Result.Fail("No hand in progress.");
        BlackjackEngine.Stand(round);
        await FinishByIdAsync(userId, round);
        return Result.Ok();
    }

    private async Task FinishByIdAsync(int userId, BlackjackRound round)
    {
        var user = await Users.GetByIdAsync(userId);
        var game = await Games.GetBySlugAsync(GameSlug.Blackjack);
        if (user != null && game != null) await FinishAsync(user, game, round);
    }

    private async Task FinishAsync(IUser<int> user, Game game, BlackjackRound round)
    {
        var payout = BlackjackEngine.Payout(round);
        await SettleAsync(user, game, round.Bet, payout, round.State.ToString(), true);
        _logger.LogInformation("Blackjack: user {UserId} finished with {State}, payout {Payout}", user.Id, round.State, payout);
    }

    private static BlackjackStateViewModel ToViewModel(BlackjackRound r)
    {
        var dealer = r.DealerHand.Select((c, i) => new CardViewModel(c.Rank, c.Suit, c.IsRed, i == 1 && !r.IsFinished)).ToList();
        var player = r.PlayerHand.Select(c => new CardViewModel(c.Rank, c.Suit, c.IsRed, false)).ToList();
        return new BlackjackStateViewModel(player, dealer, BlackjackEngine.HandValue(r.PlayerHand),
            r.IsFinished ? BlackjackEngine.HandValue(r.DealerHand) : null,
            r.IsFinished, r.State.ToString(), r.Bet, BlackjackEngine.Payout(r));
    }
}
