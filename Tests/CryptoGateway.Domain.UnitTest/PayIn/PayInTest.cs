using KsuidDotNet;
using CryptoGateway.Domain.Entities.PayIn.ValueObjects;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.Domain.UnitTest.PayIn;

public class PayInTest
{
    [Fact]
    public void Create_should_apply_PayIn_created_event()
    {
        // Arrange
        var payInExternalId = new PayInExternalId(Ksuid.NewKsuid());
        var userId = 1;
        var customerId = new CustomerId("1001");
        var txId = TxId.FromString("Test");
        var value = Money.FromDecimal(100, CurrencyType.USDT);

        // Act
        var payIn = Entities.PayIn.PayIn.Create(payInExternalId, userId, customerId, value, txId);

        // Assert
        Assert.NotNull(payIn);
        Assert.Equal(payInExternalId, payIn.PayInExternalId);
        Assert.Equal(userId, payIn.UserId);
        Assert.Equal(customerId.Value, payIn.CustomerId.Value);
        Assert.Equal(txId, payIn.TxId);
        Assert.Equal(value.Amount, payIn.Value.Amount);
        Assert.Equal(value.CurrencyType, payIn.Value.CurrencyType);
    }
}