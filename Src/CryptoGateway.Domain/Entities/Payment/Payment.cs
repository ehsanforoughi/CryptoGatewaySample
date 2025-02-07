using Bat.Core;
using CryptoGateway.Framework;
using CryptoGateway.Domain.Extensions;
using CryptoGateway.Domain.Entities.Shared;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Payment.ValueObjects;

namespace CryptoGateway.Domain.Entities.Payment;

public class Payment : AggregateRoot<PaymentId>,
    IEntity, ISoftDeleteProperty, IInsertDateProperty, IModifyDateProperty
{
    private readonly ISpotPriceProvider _spotPriceProvider;

    #region Constructors
    protected Payment() { }

    protected Payment(ISpotPriceProvider spotPriceProvider)
    {
        _spotPriceProvider = spotPriceProvider;
    }
    #endregion

    #region Private Methods
    private dynamic EstimatePayAmount(Money price, Money pay, ISpotPriceProvider spotUsdtPrice)
    {
        var usdtPrice = spotUsdtPrice.FindSpotPrice(price.CurrencyType, pay.CurrencyType).Result;
        var spotAmount = usdtPrice.Amount;
        var estimatedAmount = price.Amount / spotAmount;
        pay = Money.FromDecimal(estimatedAmount.TruncateDecimal(6), pay.CurrencyType);

        return new
        {
            SpotUsdtPriceValue = usdtPrice,
            PayValue = pay
        };
    }

    #endregion

    #region Events
    public static Payment CreatePayment(PaymentExternalId paymentExternalId, int userId,
        Money price, Money pay, CustomerId customerId, CustomerContact customerContact, OrderId orderId, OrderDescription orderDescription,
        ISpotPriceProvider spotPriceProvider)
    {
        var payment = new Payment(spotPriceProvider);
        payment.Apply(new PaymentEvents.V1.PaymentCreated
        {
            PaymentExternalId = paymentExternalId,
            UserId = userId,
            PriceAmount = price.Amount,
            PriceCurrencyCode = price.CurrencyType.ToString(),
            PayCurrencyCode = pay.CurrencyType.ToString(),
            CustomerId = customerId,
            CustomerContact = customerContact,
            OrderId = orderId,
            OrderDescription = orderDescription
        });

        return payment;
    }

    public void AssignCustodyAccount(CustodyAccount.CustodyAccount custodyAccount)
    {
        CustodyAccount = custodyAccount;
    }

    protected override void When(object @event)
    {
        switch (@event)
        {
            case PaymentEvents.V1.PaymentCreated e:
                PaymentExternalId = new PaymentExternalId(e.PaymentExternalId);
                UserId = e.UserId;
                //Type = PaymentType.CustomerOriented;
                //State = PaymentState.Waiting;

                ExpiredAt = ExpirationDateMi.FromDateTime(DateTime.Now.AddMonths(1)); //TODO: Please set a logical expiration date

                if (e.PriceAmount > 0)
                {
                    var price = Money.FromDecimal(e.PriceAmount, e.PriceCurrencyCode.Str2Enum<CurrencyType>());
                    var estimatePayAmount = EstimatePayAmount(price, Money.FromNotClearAmount(e.PayCurrencyCode), _spotPriceProvider);
                    Price = price;
                    Pay = estimatePayAmount.PayValue;
                    SpotUsdtPrice = estimatePayAmount.SpotUsdtPriceValue;
                }
                else
                {
                    Price = Money.FromNotClearAmount(e.PriceCurrencyCode);
                    Pay = Money.FromNotClearAmount(e.PayCurrencyCode);
                }

                CallBackUrl = CallBackUrl.FromString("");
                CustomerId = new CustomerId(e.CustomerId);
                CustomerContact = new CustomerContact(e.CustomerContact);
                OrderId = new OrderId(e.OrderId);
                OrderDescription = new OrderDescription(e.OrderDescription);
                break;
            case PaymentEvents.V1.PayAmountEstimationUpdated e:
                //PaymentId = e.Id;
                Pay = Money.FromDecimal(e.PayAmount, e.PayCurrencyCode.Str2Enum<CurrencyType>());
                ExpiredAt = ExpirationDateMi.FromDateTime(e.ExpirationDateMi);
                break;
        }
    }

    protected override void EnsureValidState() {}
    #endregion

    #region Properties
    public PaymentId PaymentId { get; private set; }
    public PaymentExternalId PaymentExternalId { get; private set; }
    public int UserId { get; private set; }
    public User.User User { get; private set; }
    public Money Price { get; private set; }
    public Money Pay { get; private set; }
    public Money SpotUsdtPrice { get; private set; }
    public CallBackUrl CallBackUrl { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public CustomerContact CustomerContact { get; private set; }
    public OrderId OrderId { get; private set; }
    public OrderDescription OrderDescription { get; private set; }
    public ExpirationDateMi ExpiredAt { get; private set; }
    public CustodyAccount.CustodyAccount CustodyAccount { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTime InsertDateMi { get; set; }
    public DateTime ModifyDateMi { get; set; }
    #endregion
}