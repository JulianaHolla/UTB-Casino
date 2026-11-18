using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UTB.Casino.Domain.Entities;
using UTB.Casino.Infrastructure.Identity;

namespace UTB.Casino.Infrastructure.Database.Configuration;

public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
{
    public void Configure(EntityTypeBuilder<WalletTransaction> builder)
    {
        builder.Property(t => t.Note).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Amount).HasPrecision(18, 2);

        builder.HasOne<User>(t => t.User as User).WithMany().HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(t => t.Bet).WithMany(b => b.Transactions).HasForeignKey(t => t.BetId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(t => t.TransactionType).WithMany().HasForeignKey(t => t.TransactionTypeId).OnDelete(DeleteBehavior.Restrict);
    }
}
