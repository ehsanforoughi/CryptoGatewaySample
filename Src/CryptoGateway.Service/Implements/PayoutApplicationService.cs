using Bat.Core;
using CryptoGateway.Framework;
using CryptoGateway.Service.Resources;
using CryptoGateway.Domain.Extensions;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.Domain.Entities.Wallet;
using CryptoGateway.Domain.Entities.Payout;
using CryptoGateway.Service.Contracts.Command;
using CryptoGateway.Domain.Entities.UserCredit;
using CryptoGateway.Domain.Entities.BankAccount;
using CryptoGateway.Domain.Entities.User.ValueObjects;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Payout.ValueObjects;
using CryptoGateway.Domain.Entities.Wallet.ValueObjects;
using CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

namespace CryptoGateway.Service.Implements;

public class PayoutApplicationService : IApplicationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserCreditDomainService _userCreditDomainService;
    private readonly IPayoutRepository _payoutRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IBankAccountRepository _bankAccountRepository;


    public PayoutApplicationService(IUnitOfWork unitOfWork, 
        IUserCreditDomainService userCreditDomainService, 
        IPayoutRepository payoutRepository, IUserRepository userRepository, 
        IWalletRepository walletRepository, IBankAccountRepository bankAccountRepository)
    {
        _unitOfWork = unitOfWork;
        _userCreditDomainService = userCreditDomainService;
        _payoutRepository = payoutRepository;
        _userRepository = userRepository;
        _walletRepository = walletRepository;
        _bankAccountRepository = bankAccountRepository;
    }

    private async Task<IResponse<object>> HandleCreateFiatRequest(string currentUserId, 
        PayoutCommands.V1.CreateFiatRequest cmd)
    {
        var userId = await _userRepository.GetInternalId(new UserExternalId(currentUserId));
        if (userId == null)
            return Response<object>.Error(ServiceMessages.EntityIdNotFound.Fill(currentUserId));
        
        var bankAccount = await _bankAccountRepository.Load(new BankAccountId(cmd.BankAccountId));
        if (bankAccount is null || !bankAccount.UserId.Equals(userId))
            return Response<object>.Error(ServiceMessages.EntityIdNotFoundByName
                .Fill("bank account", cmd.BankAccountId.ToString()));

        Enum.TryParse(cmd.CurrencyCode, true, out CurrencyType currencyType);
        if (!Enum.IsDefined(typeof(CurrencyType), currencyType) || currencyType != CurrencyType.IRR)
            return Response<object>.Error(ServiceMessages.InvalidEntityIdByName
                .Fill("currency type", cmd.CurrencyCode));

        var hasEnoughCredit = await _userCreditDomainService.EnsureEnoughCredit(
            (int)userId, Money.FromDecimal(cmd.Amount, cmd.CurrencyCode.Str2Enum<CurrencyType>()));
        if (!hasEnoughCredit)
            return Response<object>.Error(ServiceMessages.NotEnoughCredit.Fill("payout"));

        var payout =
            Payout.CreateFiatPayout((int)userId,
                Money.FromDecimal(cmd.Amount, cmd.CurrencyCode.Str2Enum<CurrencyType>()),
                new BankAccountId(cmd.BankAccountId));

        _payoutRepository.Add(payout);
        await _unitOfWork.Commit();

        var payoutId = payout.PayoutId.Value;
        return Response<object>.Success(payoutId, ServiceMessages.Success);
    }

    private async Task<IResponse<object>> HandleCreateCryptoRequest(string currentUserId, 
        PayoutCommands.V1.CreateCryptoRequest cmd)
    {
        var userId = await _userRepository.GetInternalId(new UserExternalId(currentUserId));
        if (userId == null)
            return Response<object>.Error(ServiceMessages.EntityIdNotFound.Fill(currentUserId));

        var wallet = await _walletRepository.Load(new WalletId(cmd.WalletId));
        if (wallet is null || !wallet.UserId.Equals(userId))
            return Response<object>.Error(ServiceMessages.EntityIdNotFoundByName
                .Fill("Wallet", cmd.WalletId.ToString()));

        Enum.TryParse(cmd.CurrencyCode, true, out CurrencyType currencyType);
        if (!Enum.IsDefined(typeof(CurrencyType), currencyType) || currencyType != CurrencyType.USDT)
            return Response<object>.Error(ServiceMessages.InvalidEntityIdByName
                .Fill("currency type", cmd.CurrencyCode));

        var hasEnoughCredit = await _userCreditDomainService.EnsureEnoughCredit(
            (int)userId, Money.FromDecimal(cmd.Amount, cmd.CurrencyCode.Str2Enum<CurrencyType>()));
        if (!hasEnoughCredit)
            return Response<object>.Error(ServiceMessages.NotEnoughCredit.Fill("payout"));

        var payout =
            Payout.CreateCryptoPayout((int)userId,
                Money.FromDecimal(cmd.Amount, cmd.CurrencyCode.Str2Enum<CurrencyType>()),
                new WalletId(cmd.WalletId));

        _payoutRepository.Add(payout);
        await _unitOfWork.Commit();

        var payoutId = payout.PayoutId.Value;
        return Response<object>.Success(payoutId, ServiceMessages.Success);
    }

    private async Task<IResponse<object>> HandleRejectRequest(string currentUserId, 
        PayoutCommands.V1.RejectPayout cmd)
    {
        var payout = await _payoutRepository.Load(new PayoutId(cmd.PayoutId));
        if (payout is null)
            return Response<object>.Error(ServiceMessages.EntityIdNotFound.Fill(cmd.PayoutId.ToString()));

        payout.Reject(new PayoutId(cmd.PayoutId), cmd.ApprovedBy, PayoutDesc.FromString(cmd.Desc!));

        await _unitOfWork.Commit();

        var payoutId = payout.PayoutId.Value;
        return Response<object>.Success(payoutId, ServiceMessages.Success);
    }

    private async Task<IResponse<object>> HandleApproveFiatRequest(string currentUserId,
        PayoutCommands.V1.ApproveFiatManualWithdrawalRequest cmd)
    {
        var payout = await _payoutRepository.Load(new PayoutId(cmd.PayoutId));
        if (payout is null)
            return Response<object>.Error(ServiceMessages.EntityIdNotFound.Fill(cmd.PayoutId.ToString()));

        if (payout.Value.CurrencyType != CurrencyType.IRR)
            return Response<object>.Error(ServiceMessages.IsNotFiat.Fill("payout", cmd.PayoutId.ToString()));

        if (payout.State != ApprovingState.Created || payout.TransferType != TransferType.FiatWithdrawal)
            return Response<object>.Error(ServiceMessages.InvalidEntityIdByName.Fill("payout", cmd.PayoutId.ToString()));

        //// Double Dispatch
        //var hasEnoughCredit = await _userCreditDomainService.EnsureEnoughCredit(payout.UserId, Money.FromDecimal(payout.Value.Amount, CurrencyType.Irr));
        //if (!hasEnoughCredit)
        //    throw new Exception($"Not enough credit for payout {cmd.PayoutId}");
        
        var user = await _userRepository.Load(payout.UserId);

        payout.ApproveManualFiatWithdrawalRequest(user, cmd.ApprovedBy, 
            PayoutDesc.FromString(cmd.Desc!),
            BankTrackingCode.FromString(cmd.BankTrackingCode!));

        await _unitOfWork.Commit();

        var payoutId = payout.PayoutId.Value;
        return Response<object>.Success(payoutId, ServiceMessages.Success);
    }

    private async Task<IResponse<object>> HandleApproveCryptoRequest(string currentUserId,
        PayoutCommands.V1.ApproveCryptoManualWithdrawalRequest cmd)
    {
        var payout = await _payoutRepository.Load(new PayoutId(cmd.PayoutId));
        if (payout is null)
            return Response<object>.Error(ServiceMessages.EntityIdNotFound.Fill(cmd.PayoutId.ToString()));

        if (payout.Value.CurrencyType == CurrencyType.IRR)
            return Response<object>.Error(ServiceMessages.IsNotCrypto.Fill("payout", cmd.PayoutId.ToString()));

        if (payout.State != ApprovingState.Created || payout.TransferType != TransferType.CryptoWithdrawal)
            return Response<object>.Error(ServiceMessages.InvalidEntityIdByName.Fill("payout", cmd.PayoutId.ToString()));

        //// Double Dispatch
        //var hasEnoughCredit = await _userCreditDomainService.EnsureEnoughCredit(payout.UserId, Money.FromDecimal(payout.Value.Amount, CurrencyType.Usdt));
        //if (!hasEnoughCredit)
        //    throw new Exception($"Not enough credit for payout {cmd.PayoutId}");

        var user = await _userRepository.Load(payout.UserId);

        payout.ApproveManualCryptoWithdrawalRequest(user, cmd.ApprovedBy, 
            PayoutDesc.FromString(cmd.Desc!), TransactionUrl.FromString(cmd.TransactionUrl!));

        await _unitOfWork.Commit();

        var payoutId = payout.PayoutId.Value;
        return Response<object>.Success(payoutId, ServiceMessages.Success);
    }

    public Task<IResponse<object>> Handle(string currentUserId, object command) =>
        command switch
        {
            PayoutCommands.V1.CreateFiatRequest cmd => HandleCreateFiatRequest(currentUserId, cmd),
            PayoutCommands.V1.CreateCryptoRequest cmd => HandleCreateCryptoRequest(currentUserId, cmd),
            PayoutCommands.V1.RejectPayout cmd => HandleRejectRequest(currentUserId, cmd),
            PayoutCommands.V1.ApproveFiatManualWithdrawalRequest cmd => HandleApproveFiatRequest(currentUserId, cmd),
            PayoutCommands.V1.ApproveCryptoManualWithdrawalRequest cmd => HandleApproveCryptoRequest(currentUserId, cmd),
            _ => throw new ArgumentOutOfRangeException(nameof(command), command, null)
        };
}