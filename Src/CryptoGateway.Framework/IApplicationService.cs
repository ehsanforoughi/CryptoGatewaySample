using Bat.Core;

namespace CryptoGateway.Framework;

public interface IApplicationService
{
    Task<IResponse<object>> Handle(string currentUserId, object command);

}