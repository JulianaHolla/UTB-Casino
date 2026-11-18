using UTB.Casino.Domain.Entities.Interfaces;

namespace UTB.Casino.Domain.Entities;

public class Entity<TKey> : IEntity<TKey> where TKey : notnull
{
    public TKey Id { get; set; } = default!;
}
