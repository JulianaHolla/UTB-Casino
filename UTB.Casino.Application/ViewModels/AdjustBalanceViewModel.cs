using System.ComponentModel.DataAnnotations;

namespace UTB.Casino.Application.ViewModels;

public class AdjustBalanceViewModel
{
    public int UserId { get; set; }

    [Range(typeof(decimal), "-100000", "100000")]
    [Display(Name = "Amount (negative removes credits)")]
    public decimal Amount { get; set; }

    [StringLength(200)]
    [Display(Name = "Reason")]
    public string? Note { get; set; }
}
