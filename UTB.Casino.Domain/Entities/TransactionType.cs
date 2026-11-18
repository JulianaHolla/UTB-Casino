namespace UTB.Casino.Domain.Entities;

/// <summary>Lookup table. Its rows correspond to the values of <see cref="Enums.TransactionTypes"/>.</summary>
public class TransactionType : Entity<int>
{
    public string Name { get; set; } = "";
}
