using Bat.Core;
using System.ComponentModel.DataAnnotations.Schema;
using CryptoGateway.Framework;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.CurrencySpotPrice.ValueObjects;

namespace CryptoGateway.Domain.Entities.CurrencySpotPrice;

[NotMapped]
public class CurrencySpotPrice : AggregateRoot<CurrencySpotPriceId>,
    IEntity, IModifyDateProperty
{
    protected override void When(object @event)
    {
        throw new NotImplementedException();
    }

    protected override void EnsureValidState()
    {
        throw new NotImplementedException();
    }

    [NotMapped]
    public CurrencySpotPriceId CurrencySpotPriceId { get; set; }
    [NotMapped]
    public CurrencyType CurrencyType { get; private set; }
    [NotMapped]
    public Money Price { get; private set; }
    [NotMapped]
    public DateTime ModifyDateMi { get; set; }
}