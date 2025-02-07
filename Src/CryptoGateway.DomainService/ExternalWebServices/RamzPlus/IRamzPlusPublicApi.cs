using CryptoGateway.DomainService.ExternalWebServices.RamzPlus.Dto;

namespace CryptoGateway.DomainService.ExternalWebServices.RamzPlus;

public interface IRamzPlusPublicApi
{
    Task<TetherMarketResultDto> GetTetherMarket();
}