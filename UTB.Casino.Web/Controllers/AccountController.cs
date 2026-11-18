using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UTB.Casino.Application.Abstraction;
using UTB.Casino.Application.ViewModels;
using UTB.Casino.Domain.Enums;

namespace UTB.Casino.Web.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accounts;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAccountService accounts, ILogger<AccountController> logger)
    { _accounts = accounts; _logger = logger; }

    [HttpGet, AllowAnonymous]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var result = await _accounts.RegisterAsync(vm);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors) ModelState.AddModelError("", error);
            return View(vm);
        }

        //sign the new user in
        var login = await _accounts.LoginAsync(new LoginViewModel { UserName = vm.UserName, Password = vm.Password });
        return login == LoginResult.Succeeded ? RedirectToAction("Index", "Home") : RedirectToAction(nameof(Login));
    }

    [HttpGet, AllowAnonymous]
    public IActionResult Login(string? returnUrl = null) => View(new LoginViewModel { ReturnUrl = returnUrl });

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var result = await _accounts.LoginAsync(vm);
        if (result == LoginResult.Succeeded)
        {
            _logger.LogInformation("User {UserName} signed in", vm.UserName);
            return !string.IsNullOrEmpty(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl)
                ? LocalRedirect(vm.ReturnUrl) : RedirectToAction("Index", "Home");
        }

        _logger.LogWarning("Failed sign-in for {UserName}", vm.UserName);
        ModelState.AddModelError("", result == LoginResult.NotAllowed ? "This account is blocked." : "Invalid username or password.");
        return View(vm);
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> Logout()
    {
        await _accounts.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    public IActionResult AccessDenied() => View();
}
