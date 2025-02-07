namespace CryptoGateway.Framework;

public interface IInternalEventHandler
{
    void Handle(object @event);
}