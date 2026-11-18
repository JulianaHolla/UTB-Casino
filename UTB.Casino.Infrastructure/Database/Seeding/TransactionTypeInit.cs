using UTB.Casino.Domain.Entities;
using UTB.Casino.Domain.Enums;

namespace UTB.Casino.Infrastructure.Database.Seeding;

internal static class TransactionTypeInit
{
    /// <summary>One row per value of the TransactionTypes enum (the enum only mirrors the table).</summary>
    public static List<TransactionType> GetTransactionTypes() =>
        Enum.GetValues<TransactionTypes>().Select(t => new TransactionType { Id = (int)t, Name = t.ToString() }).ToList();
}
