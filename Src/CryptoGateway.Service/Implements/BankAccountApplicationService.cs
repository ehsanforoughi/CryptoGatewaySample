using Bat.Core;
using CryptoGateway.Framework;
using CryptoGateway.Service.Resources;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.Service.Contracts.Command;
using CryptoGateway.Domain.Entities.BankAccount;
using CryptoGateway.Domain.Entities.User.ValueObjects;
using CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

namespace CryptoGateway.Service.Implements;

public class BankAccountApplicationService : IApplicationService
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public BankAccountApplicationService(IBankAccountRepository bankAccountRepository, 
        IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _bankAccountRepository = bankAccountRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    private async Task<IResponse<object>> HandleCreate(string currentUserId, BankAccCommands.V1.CreateBankAcc cmd)
    {
        var user = await _userRepository.Load(new UserExternalId(currentUserId));
        if (user == null)
            return Response<object>.Error(ServiceMessages.EntityIdNotFound.Fill(currentUserId));

        if (await _bankAccountRepository.Exists(user.Id, CardNumber.FromString(cmd.CardNumber)))
            return Response<object>.Error(ServiceMessages.EntityIsExistsByEntityName.Fill("bank card number", cmd.CardNumber));

        var bankAccount =
            BankAccount.Create(user.Id, BankAccountTitle.FromString(cmd.Title), 
                (BankType)cmd.BankType, CardNumber.FromString(cmd.CardNumber), Sheba.FromString(cmd.Sheba), 
                AccountNumber.FromString(cmd.AccountNumber));

        _bankAccountRepository.Add(bankAccount);
        await _unitOfWork.Commit();

        var bankAccountId = bankAccount.BankAccountId.Value;
        return Response<object>.Success(bankAccountId, ServiceMessages.Success);
    }

    private async Task<IResponse<object>> HandleUpdate(BankAccCommands.V1.EditBankAcc cmd)
    {
        var bankAccount = await _bankAccountRepository.Load(new BankAccountId(cmd.BankAccountId));
        if (bankAccount == null)
            return Response<object>.Error(ServiceMessages.EntityIdNotFound.Fill(cmd.BankAccountId.ToString()));

        bankAccount.Edit(BankAccountTitle.FromString(cmd.Title),
            (BankType)cmd.BankType, CardNumber.FromString(cmd.CardNumber), Sheba.FromString(cmd.Sheba),
            AccountNumber.FromString(cmd.AccountNumber));

        await _unitOfWork.Commit();

        var bankAccountId = bankAccount.BankAccountId.Value;
        return Response<object>.Success(bankAccountId, ServiceMessages.Success);
    }
    private async Task<IResponse<object>> HandleRemove(BankAccCommands.V1.RemoveBankAcc cmd)
    {
        var bankAccount = await _bankAccountRepository.Load(new BankAccountId(cmd.BankAccountId));
        if (bankAccount == null)
            return Response<object>.Error(ServiceMessages.EntityIdNotFound.Fill(cmd.BankAccountId.ToString()));

        bankAccount.Remove();

        await _unitOfWork.Commit();

        var bankAccountId = bankAccount.BankAccountId.Value;
        return Response<object>.Success(bankAccountId, ServiceMessages.Success);
    }
    private async Task<IResponse<object>> HandleApprove(BankAccCommands.V1.Approve cmd)
    {
        var bankAccount = await _bankAccountRepository.Load(new BankAccountId(cmd.BankAccountId));
        if (bankAccount is null)
            return Response<object>.Error(ServiceMessages.EntityIdNotFound.Fill(cmd.BankAccountId.ToString()));

        var approvedBy = await _userRepository.Load(new UserExternalId(cmd.ApprovedBy));
        if (approvedBy is null)
            return Response<object>.Error(ServiceMessages.EntityIdNotFoundByName.Fill("ApprovedBy", cmd.ApprovedBy));

        bankAccount.Approve(approvedBy.Id, BankAccountDesc.FromString(cmd.Desc));

        await _unitOfWork.Commit();

        var bankAccountId = bankAccount.BankAccountId.Value;
        return Response<object>.Success(bankAccountId, ServiceMessages.Success);
    }
    private async Task<IResponse<object>> HandleReject(BankAccCommands.V1.Reject cmd)
    {
        var bankAccount = await _bankAccountRepository.Load(new BankAccountId(cmd.BankAccountId));
        if (bankAccount is null)
            return Response<object>.Error(ServiceMessages.EntityIdNotFound.Fill(cmd.BankAccountId.ToString()));

        var approvedBy = await _userRepository.Load(new UserExternalId(cmd.ApprovedBy));
        if (approvedBy is null)
            return Response<object>.Error(ServiceMessages.EntityIdNotFoundByName.Fill("ApprovedBy", cmd.ApprovedBy));

        bankAccount.Reject(approvedBy.Id, BankAccountDesc.FromString(cmd.Desc));

        await _unitOfWork.Commit();

        var bankAccountId = bankAccount.BankAccountId.Value;
        return Response<object>.Success(bankAccountId, ServiceMessages.Success);
    }

    public Task<IResponse<object>> Handle(string currentUserId, object command) =>
        command switch
        {
            BankAccCommands.V1.CreateBankAcc cmd => HandleCreate(currentUserId, cmd),
            BankAccCommands.V1.EditBankAcc cmd => HandleUpdate(cmd),
            BankAccCommands.V1.RemoveBankAcc cmd => HandleRemove(cmd),
            BankAccCommands.V1.Approve cmd => HandleApprove(cmd),
            BankAccCommands.V1.Reject cmd => HandleReject(cmd),
            _ => throw new ArgumentOutOfRangeException(nameof(command), command, null)
        };
}