namespace UTB.Casino.Domain.Entities.Interfaces.Repository;

public interface IRepository<T, TKey> where T : IEntity<TKey> where TKey : notnull
{
    Task<T?> GetByIdAsync(TKey id);
    Task<IList<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}
