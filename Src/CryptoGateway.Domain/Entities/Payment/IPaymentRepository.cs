using CryptoGateway.Framework;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Payment.ValueObjects;

namespace CryptoGateway.Domain.Entities.Payment;

public interface IPaymentRepository : IRepository<Payment, PaymentId>
{
    Task<bool> Exists(int userId, CustomerId customerId, OrderId orderId, CurrencyType currencyType);
    Task<Payment?> Load(PaymentExternalId paymentExternalId);
    Task<Payment?> Load(int userId, CustomerId customerId);
}