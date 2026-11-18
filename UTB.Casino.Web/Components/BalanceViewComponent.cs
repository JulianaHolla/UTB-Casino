using Microsoft.AspNetCore.Mvc;
using UTB.Casino.Application.Abstraction;
using UTB.Casino.Web.Extensions;

namespace UTB.Casino.Web.Components;

public class BalanceViewComponent : ViewComponent
{
    private readonly IWalletService _wallet;
    public BalanceViewComponent(IWalletService wallet) => _wallet = wallet;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (UserClaimsPrincipal.GetUserId() is not { } id) return Content(string.Empty);
        return View(await _wallet.GetBalanceAsync(id));
    }
}
