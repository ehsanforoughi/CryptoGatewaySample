using CryptoGateway.Framework;
using CryptoGateway.Domain.Entities.User.ValueObjects;

namespace CryptoGateway.Domain.Entities.User;

public interface IUserRepository : IRepository<User, int>
{
    Task<bool> Exists(UserExternalId userExternalId);
    Task<bool> Exists(Email email);
    Task<int?> GetInternalId(UserExternalId userExternalId);
    Task<User?> Load(UserExternalId userExternalId);
}