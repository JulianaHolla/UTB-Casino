using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UTB.Casino.Domain.Entities;
using UTB.Casino.Infrastructure.Database.Configuration;
using UTB.Casino.Infrastructure.Database.Seeding;
using UTB.Casino.Infrastructure.Identity;

namespace UTB.Casino.Infrastructure.Database;

public class CasinoDbContext : IdentityDbContext<User, Role, int>
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Bet> Bets => Set<Bet>();
    public DbSet<TransactionType> TransactionTypes => Set<TransactionType>();
    public DbSet<WalletTransaction> WalletTransactions => Set<WalletTransaction>();

    public CasinoDbContext(DbContextOptions<CasinoDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new GameConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BetConfiguration());
        modelBuilder.ApplyConfiguration(new WalletTransactionConfiguration());

        //Seed data (applied by migrations, never by application code): roles first, then users, then their roles
        modelBuilder.Entity<Role>().HasData(RoleInit.GetRoles());
        modelBuilder.Entity<User>().HasData(UserInit.GetAdmin(), UserInit.GetManager());
        modelBuilder.Entity<IdentityUserRole<int>>().HasData(UserRoleInit.GetUserRoles());
        modelBuilder.Entity<TransactionType>().HasData(TransactionTypeInit.GetTransactionTypes());
        modelBuilder.Entity<Game>().HasData(GameInit.GetGames());
    }
}
