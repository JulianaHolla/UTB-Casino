using UTB.Casino.Application.GameLogic;

namespace UTB.Casino.Application.Abstraction;

public interface IBlackjackRoundStore
{
    BlackjackRound? Get(int userId);
    void Set(int userId, BlackjackRound round);
}
