using Bat.Core;
using Bat.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using CryptoGateway.DomainService.ExternalWebServices.NodeJsApi;
using CryptoGateway.DomainService.ExternalWebServices.NodeJsApi.Dto;

namespace CryptoGateway.Service.ExternalWebServices.NodeJsApi;

public class TronWeb : ITronWeb
{
    private readonly IConfiguration _configuration;
    public TronWeb(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<CreateWalletResultDto> CreateAccountAsync()
    {
        var secretKey = _configuration["NodeJsApi:SecretKey"]!;
        var url = _configuration["NodeJsApi:BaseUrl"]!;
        var body = new {};

        var header = new Dictionary<string, string> { { "secret-key", secretKey } };

        var result = await this.PostAsync($"{url}/createAccount", body, header);
        FileLoger.Message($"TronWeb/CreateWalletAsync: {result}");

        var createResult = result.DeSerializeJson<CreateWalletResultDto>();

        return new CreateWalletResultDto
        {
            PrivateKey = createResult.PrivateKey.Trim(),
            PublicKey = createResult.PublicKey.Trim(),
            Address = createResult.Address
        };
    }
    
    public async Task<object> GetAccountBalanceAsync(string walletAddressBase58)
    {
        var secretKey = _configuration["NodeJsApi:SecretKey"]!;
        var url = _configuration["NodeJsApi:BaseUrl"];
        var header = new Dictionary<string, string> { { "secret-key", secretKey } };
        var result = await HttpRequestTools.GetAsync(
            $"{url}/getBalance/{walletAddressBase58.Trim()}", null, header);

        if (result.httpStatusCode != HttpStatusCode.OK)
            return null;

        return result.response.DeSerializeJson<GetBalanceResultDto>();
    }

    public async Task<GetTransactionsResultDto?> GetTransactionsAsync(string walletAddressBase58)
    {
        var secretKey = _configuration["NodeJsApi:SecretKey"]!;
        var url = _configuration["NodeJsApi:BaseUrl"];
        var header = new Dictionary<string, string> { { "secret-key", secretKey } };
        var result = await HttpRequestTools.GetAsync(
            $"{url}/getTransactions/{walletAddressBase58.Trim()}", null, header);
        FileLoger.Message($"TronWeb/GetTransactionsAsync: {result}");

        if (result.httpStatusCode != HttpStatusCode.OK)
            return null;

        return  result.response.DeSerializeJson<GetTransactionsResultDto>();
    }

    public async Task<GetTransactionByTxIdResultDto?> GetTransactionsByTxIdAsync(string txId)
    {
        var secretKey = _configuration["NodeJsApi:SecretKey"]!;
        var url = _configuration["NodeJsApi:BaseUrl"];
        var header = new Dictionary<string, string> { { "secret-key", secretKey } };
        var result = await HttpRequestTools.GetAsync(
            $"{url}/getTransaction/{txId.Trim()}", null, header);
        FileLoger.Message($"TronWeb/GetTransactionsByTxIdAsync: {result}");

        if (result.httpStatusCode != HttpStatusCode.OK)
            return null;

        return result.response.DeSerializeJson<GetTransactionByTxIdResultDto>();
    }

    public async Task<GetTransactionsTrc20ResultDto?> GetTrc20TransactionsAsync(string walletAddressBase58)
    {
        var secretKey = _configuration["NodeJsApi:SecretKey"]!;
        var url = _configuration["NodeJsApi:BaseUrl"];
        var header = new Dictionary<string, string> { { "secret-key", secretKey } };
        var result = await HttpRequestTools.GetAsync(
            $"{url}/getTrc20Transactions/{walletAddressBase58.Trim()}", null, header);
        FileLoger.Message($"TronWeb/GetTransactionsAsync: {result}");

        if (result.httpStatusCode != HttpStatusCode.OK)
            return null;

        return result.response.DeSerializeJson<GetTransactionsTrc20ResultDto>();
    }

    public Task<object> SendTetherAsync()
    {
        throw new NotImplementedException();
    }

    public Task<object> SendTronAsync()
    {
        throw new NotImplementedException();
    }

    private static async Task<string> GetAsync(string url, Dictionary<string, string> parameter, Dictionary<string, string> header)
    {
        string responseBody = string.Empty;
        try
        {
            var handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                }
            };

            using HttpClient httpClient = new HttpClient(handler);
            string text = string.Empty;
            if (parameter.IsNotNull())
            {
                foreach (KeyValuePair<string, string> item in parameter)
                {
                    text = text + item.Key + "=" + item.Value + "&";
                }
            }

            string uriString = (string.IsNullOrWhiteSpace(text) ? url : (url + "?" + text.Substring(0, text.Length - 1)));
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(uriString));
            foreach (KeyValuePair<string, string> item2 in header)
            {
                httpRequestMessage.Headers.Add(item2.Key, item2.Value);
            }

            responseBody = await (await httpClient.SendAsync(httpRequestMessage)).Content.ReadAsStringAsync();
            return responseBody;
        }
        catch (Exception innerException)
        {
            throw new Exception(responseBody, innerException);
        }
    }

    public async Task<string> PostAsync(string url, object contentValues, Dictionary<string, string> header = null, Encoding resultEncoding = null)
    {
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(url));
        httpRequestMessage.Content = new StringContent(contentValues.SerializeToJson(new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        }), resultEncoding ?? Encoding.UTF8, "application/json");
        if (header.IsNotNull())
        {
            foreach (KeyValuePair<string, string> item in header)
            {
                httpRequestMessage.Headers.Add(item.Key, item.Value);
            }
        }

        using HttpClient httpClient = new HttpClient();
        return await (await httpClient.SendAsync(httpRequestMessage)).Content.ReadAsStringAsync();
    }
}