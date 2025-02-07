namespace CryptoGateway.Framework;

public abstract class ChildEntity<TId> : Entity<TId>
    where TId : Value<TId>
{
    protected ChildEntity() { }
    protected ChildEntity(Action<object> applier) : base(applier) { }
}