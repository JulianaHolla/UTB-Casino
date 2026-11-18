using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UTB.Casino.Domain.Entities.Interfaces.Managers;
using UTB.Casino.Domain.Entities.Interfaces.Repository;
using UTB.Casino.Domain.Entities.Interfaces.Storage;
using UTB.Casino.Infrastructure.Database;
using UTB.Casino.Infrastructure.Identity;
using UTB.Casino.Infrastructure.Managers;
using UTB.Casino.Infrastructure.Repository;
using UTB.Casino.Infrastructure.Services;

namespace UTB.Casino.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config, string webRootPath)
    {
        var connectionString = config.GetConnectionString("MySQL")
            ?? throw new InvalidOperationException("Connection string 'MySQL' is not configured (see README).");

        //fixed server version: building the model / migrations must not need a running database
        services.AddDbContext<CasinoDbContext>(o => o.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36))));

        services.AddIdentity<User, Role>(o =>
            {
                o.Password.RequiredLength = 8;
                o.Password.RequireNonAlphanumeric = false;
                o.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<CasinoDbContext>()
            .AddSignInManager<CasinoSignInManager>()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(o =>
        {
            o.LoginPath = "/Account/Login";
            o.AccessDeniedPath = "/Account/AccessDenied";
        });

        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IBetRepository, BetRepository>();
        services.AddScoped<IWalletTransactionRepository, WalletTransactionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddUnitOfWork<CasinoDbContext>();

        services.AddScoped<IAccountManager<int>, AccountIdentityManager>();

        services.AddSingleton(new FileStorageOptions { RootPath = webRootPath });
        services.AddSingleton<IFileStorage, LocalFileStorage>();
        return services;
    }

    /// <summary>Another DbContext can be given its own unit of work by calling this again.</summary>
    public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services) where TContext : DbContext
    {
        services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
        return services;
    }
}
