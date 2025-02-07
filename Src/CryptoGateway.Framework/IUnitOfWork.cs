namespace CryptoGateway.Framework;

public interface IUnitOfWork
{
    Task Commit();
    Task<bool> HasChanges();
}