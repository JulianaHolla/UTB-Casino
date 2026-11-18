namespace UTB.Casino.Application.ViewModels;

public class RouletteViewModel : GameViewModel
{
    public static readonly IList<BetTypeOption> BetTypes = new List<BetTypeOption>
    {
        new("Red (2x)", "red"), new("Black (2x)", "black"), new("Odd (2x)", "odd"), new("Even (2x)", "even"),
        new("1 to 18 (2x)", "low"), new("19 to 36 (2x)", "high"), new("Single number (36x)", "number")
    };

    public string BetType { get; set; } = "red";
    public int? Number { get; set; }
    public RouletteResultViewModel? Result { get; set; }
}
