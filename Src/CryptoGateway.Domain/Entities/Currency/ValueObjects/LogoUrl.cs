using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Currency.ValueObjects;

public class LogoUrl : Value<LogoUrl>
{
    // Satisfy the serialization requirements
    protected LogoUrl() { }

    internal LogoUrl(string logoUrl) => Value = logoUrl;

    public static LogoUrl FromString(string logoUrl)
    {
        CheckValidity(logoUrl);
        return new LogoUrl(logoUrl);
    }

    public static implicit operator string(LogoUrl self) => self.Value;
    public string Value { get; internal set; }
    public static LogoUrl NoLogoUrl => new();
    private static void CheckValidity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(LogoUrl), "LogoUrl cannot be empty");

        if (value.Length > 255)
            throw new ArgumentOutOfRangeException(nameof(LogoUrl), "LogoUrl cannot be longer that 255 characters");
    }
}