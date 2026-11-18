using System.ComponentModel.DataAnnotations;

namespace UTB.Casino.Application.ViewModels;

/// <summary>Posted by every bet form. BetType/Number are only used by roulette.</summary>
public class BetFormViewModel
{
    [Range(typeof(decimal), "0.01", "100000", ErrorMessage = "Enter a valid bet amount.")]
    public decimal Amount { get; set; }

    public string BetType { get; set; } = "red";

    [Range(0, 36, ErrorMessage = "Pick a number from 0 to 36.")]
    public int? Number { get; set; }
}
