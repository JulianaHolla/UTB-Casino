using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UTB.Casino.Application.Abstraction;
using UTB.Casino.Application.ViewModels;
using UTB.Casino.Web.Extensions;

namespace UTB.Casino.Web.Controllers;

public class HomeController : Controller
{
    private readonly IGameService _games;
    private readonly IBetService _bets;

    public HomeController(IGameService games, IBetService bets) { _games = games; _bets = bets; }

    public async Task<IActionResult> Index()
    {
        var vm = new IndexViewModel { Games = await _games.GetActiveAsync() };
        if (User.GetUserId() is { } userId)
            vm.RecentBets = await _bets.GetUserBetsAsync(userId, 5);
        return View(vm);
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
