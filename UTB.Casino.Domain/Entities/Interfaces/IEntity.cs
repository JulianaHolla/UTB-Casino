namespace UTB.Casino.Domain.Entities.Interfaces;

public interface IEntity<TKey> where TKey : notnull
{
    TKey Id { get; set; }
}
