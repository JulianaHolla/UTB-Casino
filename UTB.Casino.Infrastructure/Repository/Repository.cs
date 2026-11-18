using Microsoft.EntityFrameworkCore;
using UTB.Casino.Domain.Entities.Interfaces;
using UTB.Casino.Domain.Entities.Interfaces.Repository;
using UTB.Casino.Infrastructure.Database;

namespace UTB.Casino.Infrastructure.Repository;

/// <summary>Repositories only track changes; saving is done by the unit of work.</summary>
public class Repository<T, TKey> : IRepository<T, TKey> where T : class, IEntity<TKey> where TKey : notnull
{
    protected readonly CasinoDbContext Context;
    protected DbSet<T> Set => Context.Set<T>();

    public Repository(CasinoDbContext context) => Context = context;

    public virtual async Task<T?> GetByIdAsync(TKey id) => await Set.FindAsync(id);
    public virtual async Task<IList<T>> GetAllAsync() => await Set.ToListAsync();
    public virtual async Task AddAsync(T entity) => await Set.AddAsync(entity);
    public virtual void Update(T entity) => Set.Update(entity);
    public virtual void Remove(T entity) => Set.Remove(entity);
}
