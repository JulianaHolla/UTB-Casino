using System.ComponentModel.DataAnnotations;
using UTB.Casino.Application.Validations;
using UTB.Casino.Domain.Enums;

namespace UTB.Casino.Application.ViewModels;

public class GameEditViewModel
{
    public int Id { get; set; }

    [Required, StringLength(60, MinimumLength = 2)]
    public string Name { get; set; } = "";

    [Display(Name = "Game type")]
    public GameSlug Slug { get; set; } = GameSlug.Slots;

    [StringLength(200)]
    public string? Description { get; set; } = "";

    [Range(typeof(decimal), "0.01", "100000")]
    [Display(Name = "Min bet")]
    public decimal MinBet { get; set; } = 1;

    [Range(typeof(decimal), "0.01", "100000"), GreaterThanOrEqualTo(nameof(MinBet))]
    [Display(Name = "Max bet")]
    public decimal MaxBet { get; set; } = 100;

    [Display(Name = "Open for players")]
    public bool IsActive { get; set; } = true;

    public string? ImagePath { get; set; }
}
