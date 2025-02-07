namespace CryptoGateway.Framework;

public interface IRepository<T, in TId> where T : IAggregateRoot<TId> //where TId : Value<TId>
{
    Task<T?> Load(TId id);

    void Add(T entity);

    Task<bool> Exists(TId id);
}