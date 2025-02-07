using CryptoGateway.Framework;
using CryptoGateway.Domain.Entities.User;
using CryptoGateway.Domain.Entities.PayIn;

namespace CryptoGateway.Service.Implements;

public class PayInApplicationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPayInRepository _payInRepository;
    private readonly IUserRepository _userRepository;
    public PayInApplicationService(IUnitOfWork unitOfWork, IPayInRepository payInRepository, 
        IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _payInRepository = payInRepository;
        _userRepository = userRepository;
    }

    //private async Task<object> HandleCreateCryptoRequest(CustodyAccountCommands.V1.CreateCustodyAccount cmd)
    //{
    //    var user = await _userRepository.Load(new UserExternalId(cmd.UserId));
    //    if (user == null) throw new InvalidOperationException($"UserId with id {cmd.UserId} cannot be found");

        //var customerId = CustomerId.FromString(cmd.CustomerId);
        //var custodyAccount = CustodyAccount.Create(CustodyAccountTitle.FromString(cmd.Title),
        //    user.UserId, customerId);

        //var contractAccount =
        //    await _custodyAccountDomainService.GetLatestCustodyAccount(user.UserId, customerId);

        //payment.AssignCustodyAccount(custodyAccount);

        //_paymentRepository.Add(payment);
        //await _unitOfWork.Commit();

        //return new
        //{
        //    Id = payment.PaymentExternalId.Value,
        //    Price_Amount = payment.Price.Amount,
        //    Price_Currency = payment.Pay.CurrencyType.ToString(),
        //    Pay_Amount = payment.Pay.Amount,
        //    Pay_Currency = payment.Pay.CurrencyType.ToString(),
        //    PayAddress = custodyAccount.Address.Base58Value,
        //    Created_At = payment.InsertDateMi,
        //    Updated_At = payment.ModifyDateMi,
        //};
    //    return null;
    //}

    //public Task<object> Handle(object command) =>
    //    command switch
    //    {
    //        CustodyAccountCommands.V1.CreateCustodyAccount cmd => HandleCreateCryptoRequest(cmd),
    //        //PayInCommands.V1.RejectPayout cmd => HandleRejectRequest(cmd),
    //        _ => throw new ArgumentOutOfRangeException(nameof(command), command, null)
    //    };
}