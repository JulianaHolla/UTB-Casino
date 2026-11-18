namespace UTB.Casino.Application.ViewModels;

public class SlotsViewModel : GameViewModel
{
    public string[] Reels { get; set; } = { "🍒", "🍋", "🔔" };
    public bool HasResult { get; set; }
    public decimal Payout { get; set; }
}
