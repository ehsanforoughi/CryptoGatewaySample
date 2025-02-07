using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.Domain.UnitTest.Shared;

public class MoneyTest
{
    [Fact]
    public void Two_of_same_amount_should_be_equal()
    {
        var firstAmount = Money.FromDecimal(5, CurrencyType.IRR);
        var secondAmount = Money.FromDecimal(5, CurrencyType.IRR);

        Assert.Equal(firstAmount, secondAmount);
    }

    [Fact]
    public void Two_of_same_amount_but_different_Currencies_should_not_be_equal()
    {
        var firstAmount = Money.FromDecimal(5, CurrencyType.IRR);
        var secondAmount = Money.FromDecimal(5, CurrencyType.USDT);

        Assert.NotEqual(firstAmount, secondAmount);
    }

    [Fact]
    public void Throw_when_amount_value_is_negative()
    {
        Assert.Throws<ArgumentException>(() => Money.FromDecimal(-1, CurrencyType.USDT));
    }

    [Fact]
    public void FromString_and_FromDecimal_should_be_equal()
    {
        var firstAmount = Money.FromDecimal(5, CurrencyType.IRR);
        var secondAmount = Money.FromString("5.00", CurrencyType.IRR);


        Assert.Equal(firstAmount, secondAmount);
    }

    [Fact]
    public void Sum_of_money_gives_full_amount()
    {
        var currency1 = Money.FromDecimal(1, CurrencyType.IRR);
        var currency2 = Money.FromDecimal(2, CurrencyType.IRR);
        var currency3 = Money.FromDecimal(2, CurrencyType.IRR);

        var banknote = Money.FromDecimal(5, CurrencyType.IRR);

        Assert.Equal(banknote, currency1 + currency2 + currency3);
    }

    [Fact]
    public void Subtraction_of_two_currency_units_should_be_the_difference_of_two_values()
    {
        var firstAmount = Money.FromDecimal(3, CurrencyType.USDT);
        var secondAmount = Money.FromDecimal(1, CurrencyType.USDT);

        var banknote = Money.FromDecimal(2, CurrencyType.USDT);

        Assert.Equal(banknote, firstAmount.Subtract(secondAmount));
        Assert.Equal(banknote, firstAmount - secondAmount);
    }

    [Fact]
    public void Multiply_of_money_gives_the_correct_amount()
    {
        var currency1 = Money.FromDecimal(2, CurrencyType.IRR);
        var currency2 = Money.FromDecimal(3, CurrencyType.IRR);
        var currency3 = Money.FromDecimal(5, CurrencyType.IRR);

        var banknote = Money.FromDecimal(30, CurrencyType.IRR);

        Assert.Equal(banknote, currency1 * currency2 * currency3);
    }

    [Fact]
    public void Division_of_two_currency_units_should_be_the_correct_of_two_values()
    {
        var firstAmount = Money.FromDecimal(10, CurrencyType.USDT);
        var secondAmount = Money.FromDecimal(5, CurrencyType.USDT);

        var banknote = Money.FromDecimal(2, CurrencyType.USDT);

        Assert.Equal(banknote, firstAmount / secondAmount);
    }

    [Fact]
    public void Empty_money_should_be_able_to_be_created()
    {
        var m = Money.Empty;
        Assert.Equal(0, m.Amount);
        Assert.Equal(CurrencyType.None, m.CurrencyType);
    }

    [Fact]
    public void Throw_when_too_many_decimal_places()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Money.FromDecimal(100.123m, CurrencyType.IRR));
    }

    [Fact]
    public void Throws_on_adding_different_currencies()
    {
        var firstAmount = Money.FromDecimal(5, CurrencyType.USDT);
        var secondAmount = Money.FromDecimal(5, CurrencyType.IRR);

        Assert.Throws<CurrencyMismatchException>(() => firstAmount + secondAmount);
    }

    [Fact]
    public void Throws_on_substracting_different_currencies()
    {
        var firstAmount = Money.FromDecimal(5, CurrencyType.USDT);
        var secondAmount = Money.FromDecimal(5, CurrencyType.IRR);

        Assert.Throws<CurrencyMismatchException>(() => firstAmount - secondAmount);
    }

    [Fact]
    public void Two_of_same_amount_should_be_able_to_compare()
    {
        var firstAmount = Money.FromDecimal(5, CurrencyType.USDT);
        var secondAmount = Money.FromDecimal(10, CurrencyType.USDT);

        Assert.True(secondAmount > firstAmount);
        Assert.False(firstAmount > secondAmount);
        Assert.False(secondAmount < firstAmount);
        Assert.True(firstAmount < secondAmount);
    }
}