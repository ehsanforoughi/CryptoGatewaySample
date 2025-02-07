namespace CryptoGateway.Service.Models.Auth;

public class ExternalAuthModel
{
    public string? Provider { get; set; }
    public string? IdToken { get; set; }
}