using Bat.Core;
using KsuidDotNet;
using CryptoGateway.Framework;
using Microsoft.AspNetCore.Identity;
using CryptoGateway.Service.Resources;
using Microsoft.AspNetCore.WebUtilities;
using CryptoGateway.Service.JwtFeatures;
using CryptoGateway.Service.Models.Auth;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.Domain.Entities.Auth;
using CryptoGateway.DataAccess.DbContexts;
using CryptoGateway.Service.Contracts.Command;
using CryptoGateway.Domain.Entities.Notification;
using CryptoGateway.DataAccess.DbContexts.Extensions;
using CryptoGateway.Domain.Entities.User.ValueObjects;
using CryptoGateway.Service.Strategies.Notification.Creation;
using CryptoGateway.Domain.Entities.Notification.ValueObjects;

namespace CryptoGateway.Service.Implements;

public class AuthApplicationService : IApplicationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly UserManager<User> _userManager;
    private readonly JwtHandler _jwtHandler;
    private readonly AppDbContext _dbContext;


    public AuthApplicationService(IUnitOfWork unitOfWork, IUserRepository userRepository, 
        INotificationRepository notificationRepository, UserManager<User> userManager, 
        JwtHandler jwtHandler, AppDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _notificationRepository = notificationRepository;
        _userManager = userManager;
        _jwtHandler = jwtHandler;
        _dbContext = dbContext;
    }

    private async Task<IResponse<object>> HandleRegistration(AuthCommands.V1.Registration cmd)
    {
        await using var myTransaction = await MyDbContextTransaction.BeginTransactionAsync(_dbContext);
        try
        {
            if (await _userRepository.Exists(Email.FromString(cmd.Email)))
                return Response<object>.Error(
                    ServiceMessages.EntityIsExistsByEntityName.Fill("email", cmd.Email));

            if (cmd.Password != cmd.ConfirmPassword)
                return Response<object>.Error(ServiceMessages.AreNotEqual.Fill("password", "confirm password"));

            var user = new User(new UserExternalId(Ksuid.NewKsuid()), cmd.Email);
            var result = await _userManager.CreateAsync(user, cmd.Password);
            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var param = new Dictionary<string, string?>
                {
                    { "token", token },
                    { "email", user.Email }
                };

                if (cmd.ClientURI == null)
                    return Response<object>.Error(ServiceMessages.InvalidByName.Fill("ClientURI"));

                var callback = QueryHelpers.AddQueryString(cmd.ClientURI, param);
                var message = $"Email Confirmation Url : {callback}";

                var notification = NotificationCreationFactory.GetStrategy(NotificationType.Email)
                    .Creation(user.Id, cmd.Email, NotificationActionType.FreeTemplate, message);
                
                _notificationRepository.Add(notification);
                await _unitOfWork.Commit();

                await _userManager.AddToRoleAsync(user, "user");
                await myTransaction.CommitAsync();

                var userId = user.UserExternalId.Value;
                return Response<object>.Success(new { UserId = userId }, ServiceMessages.ConfirmationEmailSent);
            }

            var errorList = result.Errors.Select(error => error.Description).ToList();

            return Response<object>.Error(string.Join(" | ", errorList));
        }
        catch (Exception)
        {
            // Roll back the transaction if any errors occur
            await myTransaction.RollbackAsync();
            throw;
        }
    }

    private async Task<IResponse<object>> HandleLogin(AuthCommands.V1.Login cmd)
    {
        var user = await _userManager.FindByNameAsync(cmd.Email);
        if (user == null)
            return Response<object>.Error(ServiceMessages.InvalidEntityIdByName.Fill("email", cmd.Email));

        if (!await _userManager.IsEmailConfirmedAsync(user))
            return Response<object>.Error(ServiceMessages.EmailIsNotConfirmed);

        if (!await _userManager.CheckPasswordAsync(user, cmd.Password))
        {
            await _userManager.AccessFailedAsync(user);

            if (await _userManager.IsLockedOutAsync(user))
            {
                //var content = $@"Your account is locked out. To reset the password click this link: {cmd.ClientURI}";
                //var message = new Message(new string[] { cmd.Email },
                //    "Locked out account information", content, null);

                // await _emailSender.SendEmailAsync(message);

                return Response<object>.Error(ServiceMessages.UnauthorizedByParam.Fill("The account is locked out"));
            }

            return Response<object>.Error(ServiceMessages.IncorrectPassword);
        }

        if (await _userManager.GetTwoFactorEnabledAsync(user))
            return await GenerateOtpFor2StepVerification(user);

        var token = await _jwtHandler.GenerateToken(user);
        await _userManager.ResetAccessFailedCountAsync(user);
        var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault()!;

        var currentUser = new
        {
            Token = token,
            UserId = user.UserExternalId.Value,
            user.Email,
            user.UserName,
            FirstName = user.FirstName.Value,
            LastName = user.LastName.Value,
            FullName = $"{user.FirstName.Value} {user.LastName.Value}",
            Role = roleName,
            Image  = ""
        };

        return Response<object>.Success(new { CurrentUser = currentUser }, ServiceMessages.ConfirmationEmailSent);
    }

    private async Task<IResponse<object>> HandleForgotPassword(AuthCommands.V1.ForgotPassword cmd)
    {
        var user = await _userManager.FindByEmailAsync(cmd.Email);
        if (user == null)
            return Response<object>.Error(ServiceMessages.InvalidEntityIdByName.Fill("email", cmd.Email));

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var param = new Dictionary<string, string?>
            {
                {"token", token },
                {"email", cmd.Email }
            };

        var callback = QueryHelpers.AddQueryString(cmd.ClientURI, param);
        var message = $"Reset password Url : {callback}";

        var notification = NotificationCreationFactory.GetStrategy(NotificationType.Email)
            .Creation(user.Id, cmd.Email,
                NotificationActionType.FreeTemplate, message);

        _notificationRepository.Add(notification);
        await _unitOfWork.Commit();

        return Response<object>.Success(null, ServiceMessages.RestPasswordUrlEmailSent);
    }

    private async Task<IResponse<object>> HandleResetPassword(AuthCommands.V1.ResetPassword cmd)
    {
        var user = await _userManager.FindByEmailAsync(cmd.Email);
        if (user == null)
            return Response<object>.Error(ServiceMessages.InvalidEntityIdByName.Fill("email", cmd.Email));

        var resetPassResult = await _userManager.ResetPasswordAsync(user, cmd.Token, cmd.Password);
        if (!resetPassResult.Succeeded)
        {
            var errors = resetPassResult.Errors.Select(e => e.Description);

            return Response<object>.Error(string.Join(" | ", errors));
        }

        return Response<object>.Success(null, ServiceMessages.PasswordWasReset);
    }

    private async Task<IResponse<object>> HandleEmailConfirmation(AuthCommands.V1.EmailConfirmation cmd)
    {
        var user = await _userManager.FindByEmailAsync(cmd.Email);
        if (user == null)
            return Response<object>.Error(ServiceMessages.InvalidEmailConfirmation);

        var confirmResult = await _userManager.ConfirmEmailAsync(user, cmd.Token);
        if (!confirmResult.Succeeded)
            return Response<object>.Error(ServiceMessages.InvalidEmailConfirmation);

        return Response<object>.Success(null, ServiceMessages.EmailWasConfirmed);
    }

    private async Task<IResponse<object>> HandleTwoStepVerification(AuthCommands.V1.VerifyTwoStep cmd)
    {
        var user = await _userManager.FindByEmailAsync(cmd.Email);
        if (user is null)
            return Response<object>.Error(ServiceMessages.InvalidEntityIdByName.Fill("email", cmd.Email));

        var validVerification = await _userManager.VerifyTwoFactorTokenAsync(user, cmd.Provider, cmd.Token);
        if (!validVerification)
            return Response<object>.Error(ServiceMessages.InvalidTokenVerification);

        var token = await _jwtHandler.GenerateToken(user);

        var result = new { IsAuthSuccessful = true, Token = token };
        return Response<object>.Success(result, ServiceMessages.EmailWasConfirmed);
    }

    private async Task<IResponse<object>> HandleExternalLogin(AuthCommands.V1.ExternalLogin cmd)
    {
        var externalAuthModel = new ExternalAuthModel
        {
            IdToken = cmd.IdToken,
            Provider = cmd.Provider
        };

        var payload = await _jwtHandler.VerifyGoogleToken(externalAuthModel);
        if (payload == null)
            return Response<object>.Error(ServiceMessages.InvalidExternalAuthentication);

        var info = new UserLoginInfo(cmd.Provider, payload.Subject, cmd.Provider);

        var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                var newUser = new User(new UserExternalId(Ksuid.NewKsuid()), payload.Email);
                newUser.EditProfile(FirstName.FromString(payload.GivenName), LastName.FromString(payload.FamilyName),
                    NationalCode.NoNationalCode, BirthDate.NoBirthDate);
                await _userManager.CreateAsync(newUser);
                await _userManager.AddToRoleAsync(newUser, "user");
                await _userManager.AddLoginAsync(newUser, info);
                user = newUser;
            }
            else
            {
                await _userManager.AddLoginAsync(user, info);
            }
        }

        if (user == null)
            return Response<object>.Error(ServiceMessages.InvalidExternalAuthentication);

        //check for the Locked out account

        var token = await _jwtHandler.GenerateToken(user);

        var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault()!;

        var currentUser = new
        {
            Token = token,
            UserId = user.UserExternalId.Value,
            user.Email,
            user.UserName,
            FirstName = payload.GivenName,
            LastName = payload.FamilyName,
            FullName = payload.Name,
            Role = roleName,
            Image = payload.Picture,
        };

        return Response<object>.Success(new { CurrentUser = currentUser }, ServiceMessages.ConfirmationEmailSent);
    }
    private async Task<IResponse<object>> GenerateOtpFor2StepVerification(User user)
    {
        var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
        if (!providers.Contains("Email"))
            return Response<object>.Error(
                ServiceMessages.UnauthorizedByParam.Fill("Invalid 2-Step Verification Provider."));

        var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
        var message = $"Authentication token : {token}";

        var notification = NotificationCreationFactory.GetStrategy(NotificationType.Email)
            .Creation(user.Id, user.Email,
                NotificationActionType.FreeTemplate, message);

        _notificationRepository.Add(notification);
        await _unitOfWork.Commit();

        var result = new { Is2StepVerificationRequired = true, Provider = "Email" };
        return Response<object>.Success(result, ServiceMessages.ConfirmationEmailSent);
    }

    public Task<IResponse<object>> Handle(string currentUserId, object command) =>
        command switch
        {
            AuthCommands.V1.Registration cmd => HandleRegistration(cmd),
            AuthCommands.V1.Login cmd => HandleLogin(cmd),
            AuthCommands.V1.ForgotPassword cmd => HandleForgotPassword(cmd),
            AuthCommands.V1.ResetPassword cmd => HandleResetPassword(cmd),
            AuthCommands.V1.EmailConfirmation cmd => HandleEmailConfirmation(cmd),
            AuthCommands.V1.VerifyTwoStep cmd => HandleTwoStepVerification(cmd),
            AuthCommands.V1.ExternalLogin cmd => HandleExternalLogin(cmd),
            _ => throw new ArgumentOutOfRangeException(nameof(command), command, null)
        };
}