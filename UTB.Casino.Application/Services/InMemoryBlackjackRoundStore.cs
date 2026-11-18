using System.Collections.Concurrent;
using UTB.Casino.Application.Abstraction;
using UTB.Casino.Application.GameLogic;

namespace UTB.Casino.Application.Services;

/// <summary>Keeps the running hand per player (singleton, in memory).</summary>
public class InMemoryBlackjackRoundStore : IBlackjackRoundStore
{
    private readonly ConcurrentDictionary<int, BlackjackRound> _rounds = new();
    public BlackjackRound? Get(int userId) => _rounds.GetValueOrDefault(userId);
    public void Set(int userId, BlackjackRound round) => _rounds[userId] = round;
}
