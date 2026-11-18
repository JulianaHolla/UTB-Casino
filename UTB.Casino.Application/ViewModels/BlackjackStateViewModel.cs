namespace UTB.Casino.Application.ViewModels;

public record BlackjackStateViewModel(IList<CardViewModel> PlayerCards, IList<CardViewModel> DealerCards, int PlayerValue,
                                      int? DealerValue, bool IsFinished, string Outcome, decimal Bet, decimal Payout);
