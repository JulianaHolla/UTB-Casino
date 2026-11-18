using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UTB.Casino.Web.Extensions;

namespace UTB.Casino.Web.Controllers;

[Authorize]
public abstract class CasinoControllerBase : Controller
{
    protected int UserId => User.GetUserId() ?? throw new InvalidOperationException("No signed-in user.");
}
