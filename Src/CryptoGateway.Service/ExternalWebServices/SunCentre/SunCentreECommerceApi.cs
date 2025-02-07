using Bat.Core;
using Bat.Http;
using CryptoGateway.DomainService.ExternalWebServices.SunCentre;
using CryptoGateway.DomainService.ExternalWebServices.SunCentre.Dto;

namespace CryptoGateway.Service.ExternalWebServices.SunCentre;

public class SunCentreECommerceApi : ISunCentreECommerceApi
{
    public async Task<UserWalletResult?> UpdateUserWallet(int userId, decimal amount, string action, string note, int orderId)
    {
        try
        {
            var consumerKey = "123";
            var consumerSecret = "123";
            var url = "https://suncentre.ir/wp-json";
            var header = new Dictionary<string, string> { { "Content-Type", "application/json" } };
            var body = new
            {
                amount,
                action,
                consumer_key = consumerKey,
                consumer_secret = consumerSecret,
                note,
                order_id = orderId
            };

            var result = await HttpRequestTools.PutAsync($"{url}/{userId}", body, header);
            FileLoger.Message($"SunCentreECommerceApi/UpdateUserWallet: {result}");

            return result.response != "success" ? null : result.response.DeSerializeJson<UserWalletResult>();
        }
        catch (Exception e)
        {
            FileLoger.Error(e);
            return null;
        }
    }
}