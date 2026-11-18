using System.ComponentModel.DataAnnotations;

namespace UTB.Casino.Application.ViewModels;

public class BetEditViewModel
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Result { get; set; } = "";

    public string Summary { get; set; } = "";
}
