using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UTB.Casino.Domain.Entities;
using UTB.Casino.Infrastructure.Identity;

namespace UTB.Casino.Infrastructure.Database.Configuration;

public class BetConfiguration : IEntityTypeConfiguration<Bet>
{
    public void Configure(EntityTypeBuilder<Bet> builder)
    {
        builder.Property(b => b.Result).IsRequired().HasMaxLength(100);
        builder.Property(b => b.Amount).HasPrecision(18, 2);
        builder.Property(b => b.Payout).HasPrecision(18, 2);

        //User is the domain interface IUser<int>; the relation is mapped to the Identity User class
        builder.HasOne<User>(b => b.User as User).WithMany().HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(b => b.Game).WithMany(g => g.Bets).HasForeignKey(b => b.GameId).OnDelete(DeleteBehavior.Restrict);
    }
}
