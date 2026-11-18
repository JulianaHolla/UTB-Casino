namespace UTB.Casino.Domain.Entities.Interfaces.Repository;

//Commits all repository changes made through one DbContext as one unit
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
    Task ExecuteInTransactionAsync(Func<Task> action);
}
