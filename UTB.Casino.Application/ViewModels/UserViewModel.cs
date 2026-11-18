using UTB.Casino.Domain.Enums;

namespace UTB.Casino.Application.ViewModels;

public class UserViewModel
{
    public int Id { get; set; }
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public decimal Balance { get; set; }
    public bool IsBlocked { get; set; }
    public Roles Role { get; set; }
    public DateTime CreatedAt { get; set; }
}
