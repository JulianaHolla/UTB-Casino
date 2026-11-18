using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UTB.Casino.Domain.Entities;

namespace UTB.Casino.Infrastructure.Database.Configuration;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.Property(g => g.Name).IsRequired().HasMaxLength(60);
        builder.Property(g => g.Slug).HasConversion<string>().IsRequired().HasMaxLength(30);
        builder.Property(g => g.Description).IsRequired().HasMaxLength(200);
        builder.Property(g => g.MinBet).HasPrecision(18, 2);
        builder.Property(g => g.MaxBet).HasPrecision(18, 2);
        builder.Property(g => g.ImagePath).HasMaxLength(260);
        builder.HasIndex(g => g.Slug).IsUnique();
    }
}
