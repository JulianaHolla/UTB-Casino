using UTB.Casino.Domain.Entities;

namespace UTB.Casino.Application.ViewModels;

public class WalletViewModel
{
    public decimal Balance { get; set; }
    public IList<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
}
