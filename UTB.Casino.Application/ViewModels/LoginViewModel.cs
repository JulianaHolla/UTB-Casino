using System.ComponentModel.DataAnnotations;

namespace UTB.Casino.Application.ViewModels;

public class LoginViewModel
{
    [Required, Display(Name = "Username")]
    public string UserName { get; set; } = "";

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}
