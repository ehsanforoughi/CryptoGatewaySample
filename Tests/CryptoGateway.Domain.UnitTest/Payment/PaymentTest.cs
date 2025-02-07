using Moq;
using KsuidDotNet;
using CryptoGateway.Domain.UnitTest.Shared;
using CryptoGateway.Domain.Entities.Shared;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Payment.ValueObjects;

namespace CryptoGateway.Domain.UnitTest.Payment;

public class PaymentTest
{
    private readonly ISpotPriceProvider _spotPrice = new FakeSpotPrice();

    [Fact]
    public void Create_should_apply_payment_created_event()
    {
        // Arrange
        var paymentExternalId = new PaymentExternalId(Ksuid.NewKsuid());
        var userId = 1;
        var price = Money.FromDecimal(100000, CurrencyType.IRR);
        var payPrice = Money.FromNotClearAmount(CurrencyType.USDT);
        var customerId = CustomerId.FromString("1001");
        var customerContact = CustomerContact.FromString("a@a.com");
        var orderId = OrderId.FromString("GP-1000");
        var orderDescription = OrderDescription.FromString("Apple AirPods Pro");

        // Act
        var payment = Entities.Payment.Payment.CreatePayment(paymentExternalId, userId,
            price, payPrice, customerId, customerContact, orderId, orderDescription, _spotPrice);
        var payAmount = payment.Price.Amount / payment.SpotUsdtPrice.Amount;

        // Assert
        Assert.Equal(userId, payment.UserId);
        Assert.Equal(price, payment.Price);
        Assert.Equal(decimal.Round(payAmount, 4), decimal.Round(payment.Pay.Amount, 4));
        Assert.True(payment.Pay.Amount > 0);
    }


    [Fact]
    public void Create_a_priceless_payment_should_be_done()
    {
        // Arrange
        var paymentExternalId = new PaymentExternalId(Ksuid.NewKsuid());
        var userId = 1;
        var price = Money.FromNotClearAmount(CurrencyType.IRR);
        var payPrice = Money.FromNotClearAmount(CurrencyType.USDT);
        var customerId = CustomerId.FromString("1001");
        var customerContact = CustomerContact.FromString("a@a.com");
        var orderId = OrderId.FromString("GP-1000");
        var orderDescription = OrderDescription.FromString("Apple AirPods Pro");

        // Act
        var payment = Entities.Payment.Payment.CreatePayment(paymentExternalId, userId,
            price, payPrice, customerId, customerContact, orderId, orderDescription, _spotPrice);

        // Assert
        Assert.Equal(userId, payment.UserId);
        Assert.Equal(price, payment.Price);
        Assert.True(payment.Pay.Amount == 0);
    }
}