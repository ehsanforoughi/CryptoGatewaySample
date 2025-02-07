using Bat.Core;
using Bat.Http;
using System.Net;
using Microsoft.Extensions.Configuration;
using CryptoGateway.DomainService.ExternalWebServices.RamzPlus;
using CryptoGateway.DomainService.ExternalWebServices.RamzPlus.Dto;

namespace CryptoGateway.Service.ExternalWebServices.RamzPlus;

public class RamzPlusPublicApi : IRamzPlusPublicApi
{
    private readonly IConfiguration _configuration;
    public RamzPlusPublicApi(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<TetherMarketResultDto> GetTetherMarket()
    {
        var url = _configuration["RamzPlusPublicApi:BaseUrl"];
        var result = await HttpRequestTools.GetAsync($"{url}/TetherMarket");
        FileLoger.Message($"RamzPlusPublicApi/TetherMarket: {result}");

        if (result.httpStatusCode != HttpStatusCode.OK)
            return null;

        return result.response.DeSerializeJson<TetherMarketResultDto>();
    }
}