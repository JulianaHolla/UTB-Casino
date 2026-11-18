using UTB.Casino.Domain.Entities;

namespace UTB.Casino.Application.ViewModels;

public abstract class GameViewModel
{
    public Game Game { get; set; } = new();
    public decimal Amount { get; set; }
    public string? Message { get; set; }
    public bool IsError { get; set; }
}
