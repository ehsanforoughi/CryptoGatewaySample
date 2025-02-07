using Bat.Core;
using CryptoGateway.Framework;
using CryptoGateway.Service.Resources;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.Domain.Entities.ApiKey;
using CryptoGateway.Service.Contracts.Command;
using CryptoGateway.Domain.Entities.User.ValueObjects;
using CryptoGateway.Domain.Entities.ApiKey.ValueObjects;

namespace CryptoGateway.Service.Implements;

public class ApiKeyApplicationService : IApplicationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApiKeyRepository _apiKeyRepository;
    private readonly IUserRepository _userRepository;


    public ApiKeyApplicationService(IUnitOfWork unitOfWork, IApiKeyRepository apiKeyRepository, 
        IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _apiKeyRepository = apiKeyRepository;
        _userRepository = userRepository;
    }

    private async Task<IResponse<object>> HandleCreate(string currentUserId, ApiKeyCommands.V1.GenerateApiKey cmd)
    {
        var user = await _userRepository.Load(new UserExternalId(currentUserId));
        if (user == null)
            return Response<object>.Error(ServiceMessages.EntityIdNotFound.Fill(currentUserId));

        if (await _apiKeyRepository.Exists(user.Id))
            return Response<object>.Error(ServiceMessages.UserHasActiveApiKey);


        var apiKey = ApiKey.Generate(user.Id, KeyValue.NoKeyValue);
        _apiKeyRepository.Add(apiKey);
        await _unitOfWork.Commit();

        return Response<object>.Success(null, ServiceMessages.Success);
    }

    public Task<IResponse<object>> Handle(string currentUserId, object command) =>
        command switch
        {
            ApiKeyCommands.V1.GenerateApiKey cmd => HandleCreate(currentUserId, cmd),
            _ => throw new ArgumentOutOfRangeException(nameof(command), command, null)
        };
}