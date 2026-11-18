namespace UTB.Casino.Domain.Entities.Interfaces.Repository;

public interface IUserRepository
{
    Task<IUser<int>?> GetByIdAsync(int id);
    Task<IList<IUser<int>>> GetAllAsync();
}
