namespace CryptoGateway.DomainService.ExternalWebServices.NodeJsApi.Dto;

public class CreateWalletResultDto
{
    public string PrivateKey { get; set; }
    public string PublicKey { get; set; }
    public WalletAddress Address { get; set; }

    public class WalletAddress
    {
        public string Base58 { get; set; }
        public string Hex { get; set; }
    }
}