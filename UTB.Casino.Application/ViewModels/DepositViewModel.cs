using System.ComponentModel.DataAnnotations;

namespace UTB.Casino.Application.ViewModels;

public class DepositViewModel
{
    [Range(typeof(decimal), "1", "1000", ErrorMessage = "Add between 1 and 1000 credits.")]
    public decimal Amount { get; set; } = 100;
}
