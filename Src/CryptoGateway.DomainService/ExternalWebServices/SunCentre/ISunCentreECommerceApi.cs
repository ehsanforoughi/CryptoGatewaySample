using CryptoGateway.DomainService.ExternalWebServices.SunCentre.Dto;

namespace CryptoGateway.DomainService.ExternalWebServices.SunCentre;

public interface ISunCentreECommerceApi
{
    Task<UserWalletResult?> UpdateUserWallet(int userId, decimal amount, string action, string note, int orderId);
}