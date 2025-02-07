using Bat.Core;
using Microsoft.Extensions.Configuration;

namespace CryptoGateway.Infrastructure.EmailGateway;

public interface IEmailGatewayAdapter
{
    IResponse<bool> Send(IServiceProvider serviceProvider, string actionType, string receiver, string subject, string text);
}