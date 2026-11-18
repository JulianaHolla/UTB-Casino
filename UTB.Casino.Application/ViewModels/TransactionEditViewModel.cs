using System.ComponentModel.DataAnnotations;

namespace UTB.Casino.Application.ViewModels;

public class TransactionEditViewModel
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    public string Note { get; set; } = "";

    public string Summary { get; set; } = "";
}
