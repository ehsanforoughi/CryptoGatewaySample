using Microsoft.EntityFrameworkCore;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.DataAccess.DbContexts;
using CryptoGateway.Domain.Entities.User.ValueObjects;

namespace CryptoGateway.DataAccess.Repositories;

public class UserRepository : IUserRepository, IDisposable
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
        => _dbContext = dbContext;

    public void Add(User entity)
        => _dbContext.User.Add(entity);

    public async Task<bool> Exists(int id)
        => await _dbContext.User.AnyAsync(x => x.Id.Equals(id));
    public async Task<bool> Exists(UserExternalId userExternalId)
        => await _dbContext.User.AnyAsync(x => x.UserExternalId.Equals(userExternalId));
    public async Task<bool> Exists(Email email)
        => await _dbContext.User.AnyAsync(x => x.Email.Equals(email));

    public async Task<int?> GetInternalId(UserExternalId userExternalId)
    {
        var user = await _dbContext.User
            //.FirstOrDefaultAsync(x => x.UserExternalId.Equals(userExternalId));
            .FirstOrDefaultAsync(x => x.UserExternalId.Value.ToUpper() == userExternalId.Value.ToUpper());

        return user?.Id;
    }

    public async Task<User?> Load(int id)
        => await _dbContext.User
            .Include(x => x.UserCredits)
            .ThenInclude(x => x.Transactions)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
    public async Task<User?> Load(UserExternalId userExternalId)
        => (await _dbContext.User.Include(x => x.UserCredits)
            .FirstOrDefaultAsync(x => x.UserExternalId.Value.Equals(userExternalId.Value)))!;
    public void Dispose() => _dbContext.Dispose();
}