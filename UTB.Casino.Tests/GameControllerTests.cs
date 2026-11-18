using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using UTB.Casino.Application.Abstraction;
using UTB.Casino.Application.Common;
using UTB.Casino.Application.ViewModels;
using UTB.Casino.Domain.Entities;
using UTB.Casino.Domain.Enums;
using UTB.Casino.Web.Areas.Admin.Controllers;

namespace UTB.Casino.Tests;

/// Admin GameController tests: the controller talks to the database through IGameService (mocked here).
/// Every test builds its own controller and its own mock, so nothing can leak from one test into another.
public class GameControllerTests
{
    private static (GameController Controller, Mock<IGameService> Service) CreateController()
    {
        var service = new Mock<IGameService>();
        var controller = new GameController(service.Object, NullLogger<GameController>.Instance)
        {
            TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
        };
        return (controller, service);
    }

    private static GameEditViewModel ValidVm() => new() { Name = "Slots", Slug = GameSlug.Slots, MinBet = 1, MaxBet = 100 };

    [Fact]
    public async Task Select_returns_all_games_from_service()
    {
        var (controller, service) = CreateController();
        var games = new List<Game> { new() { Id = 1, Name = "A" }, new() { Id = 2, Name = "B" } };
        service.Setup(s => s.GetAllAsync()).ReturnsAsync(games);

        var result = await controller.Select();

        var view = Assert.IsType<ViewResult>(result);
        Assert.Equal(2, Assert.IsAssignableFrom<IList<Game>>(view.Model).Count);
        service.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Create_with_invalid_model_does_not_call_service()
    {
        var (controller, service) = CreateController();
        controller.ModelState.AddModelError("Name", "Required");

        var result = await controller.Create(ValidVm(), null);

        Assert.IsType<ViewResult>(result);
        service.Verify(s => s.CreateAsync(It.IsAny<GameEditViewModel>(), It.IsAny<Stream?>(), It.IsAny<string?>()), Times.Never);
    }

    [Fact]
    public async Task Create_valid_calls_service_and_redirects_to_Select()
    {
        var (controller, service) = CreateController();
        service.Setup(s => s.CreateAsync(It.IsAny<GameEditViewModel>(), It.IsAny<Stream?>(), It.IsAny<string?>())).ReturnsAsync(Result.Ok());

        var result = await controller.Create(ValidVm(), null);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(GameController.Select), redirect.ActionName);
        service.Verify(s => s.CreateAsync(It.Is<GameEditViewModel>(d => d.Slug == GameSlug.Slots && d.MaxBet == 100), null, null), Times.Once);
    }

    [Fact]
    public async Task Create_service_failure_shows_error_on_form()
    {
        var (controller, service) = CreateController();
        service.Setup(s => s.CreateAsync(It.IsAny<GameEditViewModel>(), It.IsAny<Stream?>(), It.IsAny<string?>()))
               .ReturnsAsync(Result.Fail("A game of this type already exists."));

        var result = await controller.Create(ValidVm(), null);

        Assert.IsType<ViewResult>(result);
        Assert.False(controller.ModelState.IsValid);
    }

    [Fact]
    public async Task Edit_unknown_game_returns_NotFound()
    {
        var (controller, service) = CreateController();
        service.Setup(s => s.GetByIdAsync(42)).ReturnsAsync((Game?)null);

        Assert.IsType<NotFoundResult>(await controller.Edit(42));
    }

    [Fact]
    public async Task Edit_valid_updates_through_service()
    {
        var (controller, service) = CreateController();
        service.Setup(s => s.UpdateAsync(It.IsAny<GameEditViewModel>(), It.IsAny<Stream?>(), It.IsAny<string?>())).ReturnsAsync(Result.Ok());
        var vm = ValidVm();
        vm.Id = 7;

        var result = await controller.Edit(vm, null);

        Assert.IsType<RedirectToActionResult>(result);
        service.Verify(s => s.UpdateAsync(It.Is<GameEditViewModel>(d => d.Id == 7), null, null), Times.Once);
    }

    [Fact]
    public async Task Delete_failure_is_reported_in_TempData()
    {
        var (controller, service) = CreateController();
        service.Setup(s => s.DeleteAsync(3)).ReturnsAsync(Result.Fail("This game has bets and cannot be deleted."));

        await controller.Delete(3);

        Assert.Contains("bets", (string)controller.TempData["Error"]!);
    }
}
