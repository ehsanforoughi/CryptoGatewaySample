using Bat.Core;
using CryptoGateway.Framework;
using CryptoGateway.Service.Resources;
using CryptoGateway.Domain.Extensions;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.Domain.Entities.Wallet;
using CryptoGateway.Domain.Entities.Currency;
using CryptoGateway.Service.Contracts.Command;
using CryptoGateway.Domain.Entities.User.ValueObjects;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Wallet.ValueObjects;

namespace CryptoGateway.Service.Implements;

public class WalletApplicationService : IApplicationService
{
    private readonly IWalletRepository _walletRepository;
    private readonly ICurrencyRepository _currencyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public WalletApplicationService(IWalletRepository repository, 
        IUnitOfWork unitOfWork, ICurrencyRepository currencyRepository, 
        IUserRepository userRepository)
    {
        _walletRepository = repository;
        _unitOfWork = unitOfWork;
        _currencyRepository = currencyRepository;
        _userRepository = userRepository;
    }

    private async Task<IResponse<object>> HandleCreate(string currentUserId, 
        WalletCommands.V1.CreateWallet cmd)
    {
        var user = await _userRepository.Load(new UserExternalId(currentUserId));
        if (user == null)
            return Response<object>.Error(ServiceMessages.EntityIdNotFound.Fill(currentUserId));

        var currency = await _currencyRepository.Load(cmd.CurrencyType.Str2Enum<CurrencyType>());
        if (currency is null || currency.IsFiat)
            return Response<object>.Error(ServiceMessages.InvalidEntityIdByName
                .Fill("currency",cmd.CurrencyType));

        if (await _walletRepository.Exists(user.Id, WalletAddress.FromString(cmd.Address)))
            return Response<object>.Error(ServiceMessages.EntityIsExistsByEntityName.Fill("wallet address", cmd.Address.ToString()));

        var wallet =
            Wallet.Create(user.Id, WalletTitle.FromString(cmd.Title),
                new CurrencyId(currency.CurrencyId), Network.FromString(cmd.Network), WalletAddress.FromString(cmd.Address), 
                MemoAddress.FromString(cmd.MemoAddress), TagAddress.FromString(cmd.TagAddress));

        _walletRepository.Add(wallet);
        await _unitOfWork.Commit();

        var walletId = wallet.WalletId.Value;
        return Response<object>.Success(walletId, ServiceMessages.Success);
    }
    private async Task<IResponse<object>> HandleUpdate(string currentUserId, 
        WalletCommands.V1.EditWallet cmd)
    {
        var wallet = await _walletRepository.Load(new WalletId(cmd.WalletId));
        if (wallet == null)
            return Response<object>.Error(ServiceMessages.EntityIdNotFound.Fill(cmd.WalletId.ToString()));

        var currency = await _currencyRepository.Load(cmd.CurrencyType.Str2Enum<CurrencyType>());
        if (currency is null || currency.IsFiat)
            return Response<object>.Error(ServiceMessages.InvalidEntityIdByName
                .Fill("currency", cmd.CurrencyType));

        wallet.Edit(WalletTitle.FromString(cmd.Title),
            new CurrencyId(currency.CurrencyId), Network.FromString(cmd.Network), WalletAddress.FromString(cmd.Address),
            MemoAddress.FromString(cmd.MemoAddress), TagAddress.FromString(cmd.TagAddress));

        await _unitOfWork.Commit();

        var walletId = wallet.WalletId.Value;
        return Response<object>.Success(walletId, ServiceMessages.Success);
    }
    private async Task<IResponse<object>> HandleRemove(string currentUserId, 
        WalletCommands.V1.RemoveWallet cmd)
    {
        var wallet = await _walletRepository.Load(new WalletId(cmd.WalletId));
        if (wallet == null)
            return Response<object>.Error(ServiceMessages.EntityIdNotFound.Fill(cmd.WalletId.ToString()));

        wallet.Remove();

        await _unitOfWork.Commit();

        var walletId = wallet.WalletId.Value;
        return Response<object>.Success(walletId, ServiceMessages.Success);
    }
    private async Task<IResponse<object>> HandleApprove(string currentUserId, 
        WalletCommands.V1.Approve cmd)
    {
        var wallet = await _walletRepository.Load(new WalletId(cmd.WalletId));
        if (wallet is null)
            return Response<object>.Error(ServiceMessages.EntityIdNotFound.Fill(cmd.WalletId.ToString()));

        wallet.Approve(cmd.ApprovedBy, WalletDesc.FromString(cmd.Desc));

        await _unitOfWork.Commit();

        var walletId = wallet.WalletId.Value;
        return Response<object>.Success(walletId, ServiceMessages.Success);
    }
    private async Task<IResponse<object>> HandleReject(string currentUserId, 
        WalletCommands.V1.Reject cmd)
    {
        var wallet = await _walletRepository.Load(new WalletId(cmd.WalletId));
        if (wallet is null)
            return Response<object>.Error(ServiceMessages.EntityIdNotFound.Fill(cmd.WalletId.ToString()));

        wallet.Reject(cmd.ApprovedBy, WalletDesc.FromString(cmd.Desc));

        await _unitOfWork.Commit();

        var walletId = wallet.WalletId.Value;
        return Response<object>.Success(walletId, ServiceMessages.Success);
    }

    public Task<IResponse<object>> Handle(string currentUserId, object command) =>
        command switch
        {
            WalletCommands.V1.CreateWallet cmd => HandleCreate(currentUserId, cmd),
            WalletCommands.V1.EditWallet cmd => HandleUpdate(currentUserId, cmd),
            WalletCommands.V1.RemoveWallet cmd => HandleRemove(currentUserId, cmd),
            WalletCommands.V1.Approve cmd => HandleApprove(currentUserId, cmd),
            WalletCommands.V1.Reject cmd => HandleReject(currentUserId, cmd),
            _ => throw new ArgumentOutOfRangeException(nameof(command), command, null)
        };
}