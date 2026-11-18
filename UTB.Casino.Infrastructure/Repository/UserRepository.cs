using Microsoft.EntityFrameworkCore;
using UTB.Casino.Domain.Entities.Interfaces;
using UTB.Casino.Domain.Entities.Interfaces.Repository;
using UTB.Casino.Infrastructure.Database;

namespace UTB.Casino.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly CasinoDbContext _context;
    public UserRepository(CasinoDbContext context) => _context = context;

    public async Task<IUser<int>?> GetByIdAsync(int id) => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<IList<IUser<int>>> GetAllAsync() =>
        (await _context.Users.OrderBy(u => u.UserName).ToListAsync()).Cast<IUser<int>>().ToList();
}
