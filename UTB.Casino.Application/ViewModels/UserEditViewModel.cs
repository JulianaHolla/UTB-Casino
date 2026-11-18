using System.ComponentModel.DataAnnotations;
using UTB.Casino.Domain.Enums;

namespace UTB.Casino.Application.ViewModels;

/// <summary>Admin edit of a user. There is deliberately no password / hash field.</summary>
public class UserEditViewModel
{
    public int Id { get; set; }

    [Required, StringLength(30, MinimumLength = 3), RegularExpression(@"^[A-Za-z0-9_.-]+$")]
    [Display(Name = "Username")]
    public string UserName { get; set; } = "";

    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = "";

    [Required, StringLength(60, MinimumLength = 2)]
    [Display(Name = "Display name")]
    public string DisplayName { get; set; } = "";

    public Roles Role { get; set; } = Roles.Customer;

    [Display(Name = "Blocked")]
    public bool IsBlocked { get; set; }

    public decimal Balance { get; set; }
}
