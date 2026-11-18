using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UTB.Casino.Domain.Entities;

namespace UTB.Casino.Infrastructure.Database.Configuration;

public class TransactionTypeConfiguration : IEntityTypeConfiguration<TransactionType>
{
    public void Configure(EntityTypeBuilder<TransactionType> builder)
    {
        builder.Property(t => t.Id).ValueGeneratedNever();   // ids come from the TransactionTypes enum
        builder.Property(t => t.Name).IsRequired().HasMaxLength(30);
        builder.HasIndex(t => t.Name).IsUnique();
    }
}
