
namespace CryptoGateway.Domain.Entities.ApiKey;

public static class ApiKeyEvents
{
    public static class V1
    {
        public class ApiKeyGenerated
        {
            public int UserId { get; set; }
            public string KeyValue { get; set; }
        }
    }
}