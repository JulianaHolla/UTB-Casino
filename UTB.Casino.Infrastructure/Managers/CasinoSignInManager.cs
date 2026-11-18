using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UTB.Casino.Infrastructure.Identity;

namespace UTB.Casino.Infrastructure.Managers;

/// <summary>Blocked players cannot sign in.</summary>
public class CasinoSignInManager : SignInManager<User>
{
    public CasinoSignInManager(UserManager<User> userManager, IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<User> claimsFactory, IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<User>> logger, IAuthenticationSchemeProvider schemes,
        IUserConfirmation<User> confirmation)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation) { }

    public override async Task<bool> CanSignInAsync(User user) => !user.IsBlocked && await base.CanSignInAsync(user);
}
