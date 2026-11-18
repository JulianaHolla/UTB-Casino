using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UTB.Casino.Application.Abstraction;
using UTB.Casino.Application.ViewModels;
using UTB.Casino.Domain.Enums;

namespace UTB.Casino.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Manager))]
public class GameController : Controller
{
    private readonly IGameService _games;
    private readonly ILogger<GameController> _logger;

    public GameController(IGameService games, ILogger<GameController> logger) { _games = games; _logger = logger; }

    public async Task<IActionResult> Select() => View(await _games.GetAllAsync());

    [HttpGet]
    public IActionResult Create() => View(new GameEditViewModel());

    //the uploaded image is not part of the view model (the view model must not depend on ASP.NET types)
    [HttpPost]
    public async Task<IActionResult> Create(GameEditViewModel vm, IFormFile? image)
    {
        if (!ModelState.IsValid) return View(vm);

        await using var stream = image?.OpenReadStream();
        var result = await _games.CreateAsync(vm, stream, image?.FileName);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors) ModelState.AddModelError("", error);
            return View(vm);
        }
        _logger.LogInformation("Game {Slug} created from admin", vm.Slug);
        return RedirectToAction(nameof(Select));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var game = await _games.GetByIdAsync(id);
        if (game == null) return NotFound();
        return View(new GameEditViewModel
        {
            Id = game.Id, Name = game.Name, Slug = game.Slug, Description = game.Description,
            MinBet = game.MinBet, MaxBet = game.MaxBet, IsActive = game.IsActive, ImagePath = game.ImagePath
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(GameEditViewModel vm, IFormFile? image)
    {
        if (!ModelState.IsValid) return View(vm);

        await using var stream = image?.OpenReadStream();
        var result = await _games.UpdateAsync(vm, stream, image?.FileName);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors) ModelState.AddModelError("", error);
            return View(vm);
        }
        _logger.LogInformation("Game {GameId} edited from admin", vm.Id);
        return RedirectToAction(nameof(Select));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _games.DeleteAsync(id);
        TempData[result.Succeeded ? "Success" : "Error"] = result.Succeeded ? "Game deleted." : result.Error;
        return RedirectToAction(nameof(Select));
    }
}
