using UTB.Casino.Domain.Entities;

namespace UTB.Casino.Application.ViewModels;

public class IndexViewModel
{
    public IList<Game> Games { get; set; } = new List<Game>();
    public IList<Bet> RecentBets { get; set; } = new List<Bet>();
}
