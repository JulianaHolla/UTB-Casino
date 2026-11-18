using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UTB.Casino.Infrastructure.Identity;

namespace UTB.Casino.Infrastructure.Database.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.DisplayName).IsRequired().HasMaxLength(60);
        builder.Property(u => u.DateOfBirth).HasColumnType("date");
        builder.Property(u => u.Balance).HasPrecision(18, 2);
    }
}
