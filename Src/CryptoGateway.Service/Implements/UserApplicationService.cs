using Bat.Core;
using KsuidDotNet;
using CryptoGateway.Framework;
using CryptoGateway.Service.Resources;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.Service.Contracts.Command;
using CryptoGateway.Domain.Entities.User.ValueObjects;

namespace CryptoGateway.Service.Implements;

public class UserApplicationService : IApplicationService
{
    private readonly IUserRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UserApplicationService(IUserRepository repository, 
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    private async Task<IResponse<object>> HandleRegistration(string currentUserId, 
        UserCommands.V1.RegisterUser cmd)
    {
        if (await _repository.Exists(Email.FromString(cmd.Email)))
            return Response<object>.Error(ServiceMessages.EntityIsExistsByEntityName.Fill("email", cmd.Email));

        if (cmd.Password != cmd.ConfirmPassword)
            return Response<object>.Error(ServiceMessages.AreNotEqual.Fill("password", "confirm password"));

        var user = new User(new UserExternalId(Ksuid.NewKsuid()), Email.FromString(cmd.Email));

        _repository.Add(user);
        await _unitOfWork.Commit();

        var userId = user.UserExternalId.Value;
        return Response<object>.Success(userId, ServiceMessages.Success);
    }

    public Task<IResponse<object>> Handle(string currentUserId, object command) =>
        command switch
        {
            //UserCommands.V1.RegisterUser cmd => HandleRegistration(currentUserId, cmd),
            //UserCommands.V1.EditUserProfile cmd => HandleEditProfile(currentUserId, cmd),
            //UserCommands.V1.EditPassword cmd => HandleEditPassword(currentUserId, cmd),
            //UserCommands.V1.SetMobileNumber cmd => HandleSetMobileNumber(currentUserId, cmd),
            _ => throw new ArgumentOutOfRangeException(nameof(command), command, null)
        };
}