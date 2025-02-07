using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Payment.ValueObjects;

public class CallBackUrl : Value<CallBackUrl>
{
    public Uri Value { get; internal set; }
    protected CallBackUrl() { }

    internal CallBackUrl(Uri value)
    {
        //if (value == default)
        //    throw new ArgumentNullException(nameof(CallBackUrl), "Call back url cannot be empty");

        Value = value;
    }

    public static CallBackUrl? FromString(string url) => string.IsNullOrWhiteSpace(url) ? null : new(new Uri(url));

    public static implicit operator Uri(CallBackUrl url) => url.Value;

    public static CallBackUrl NoCallBackUrl => new();
}