using Microsoft.EntityFrameworkCore;
using CryptoGateway.DataAccess.DbContexts;
using CryptoGateway.Domain.Entities.Payment;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.Payment.ValueObjects;

namespace CryptoGateway.DataAccess.Repositories;

public class PaymentRepository : IPaymentRepository, IDisposable
{
    private readonly AppDbContext _dbContext;

    public PaymentRepository(AppDbContext dbContext)
        => _dbContext = dbContext;

    public void Add(Payment entity)
        => _dbContext.Payment.Add(entity);

    public async Task<bool> Exists(PaymentId id)
        => await _dbContext.Payment.FirstOrDefaultAsync(x => x.PaymentId.Equals(id)) != null;

    public async Task<bool> Exists(int userId, CustomerId customerId, OrderId orderId, CurrencyType currencyType)
        => await _dbContext.Payment.FirstOrDefaultAsync(x => x.UserId.Equals(userId) &&
                                                             x.CustomerId.Equals(customerId) &&
                                                             x.OrderId.Equals(orderId) &&
                                                             x.Pay.CurrencyType.Equals(currencyType)) != null;

    public async Task<Payment?> Load(PaymentId id)
        => await _dbContext
            .Payment
            .Include(x => x.CustodyAccount)
            .ThenInclude(x => x.ContractAccount)
            .FirstOrDefaultAsync(x => x.PaymentId.Equals(id));

    public async Task<Payment?> Load(PaymentExternalId paymentExternalId)
        => await _dbContext.Payment
            .FirstOrDefaultAsync(x => x.PaymentExternalId.Value.Equals(paymentExternalId.Value));

    public async Task<Payment?> Load(int userId, CustomerId customerId)
        => await _dbContext.Payment.Include(x => x.CustodyAccount)
            .OrderByDescending(x => x.InsertDateMi)
            .FirstOrDefaultAsync(x => x.UserId.Equals(userId) &&
                                      x.CustomerId.Equals(customerId));

    public void Dispose() => _dbContext.Dispose();
}