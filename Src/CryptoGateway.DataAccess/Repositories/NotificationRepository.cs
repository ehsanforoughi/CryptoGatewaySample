using CryptoGateway.DataAccess.DbContexts;
using CryptoGateway.Domain.Entities.Notification;
using CryptoGateway.Domain.Entities.Notification.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CryptoGateway.DataAccess.Repositories;

public class NotificationRepository : INotificationRepository, IDisposable
{
    private readonly AppDbContext _dbContext;
    public NotificationRepository(AppDbContext dbContext) => _dbContext = dbContext;

    public async Task<Notification?> Load(NotificationId id)
        => await _dbContext.Notification.FirstOrDefaultAsync(x => x.NotificationId.Equals(id));

    public void Add(Notification entity)
        => _dbContext.Notification.Add(entity);

    public async Task<bool> Exists(NotificationId id)
        => await _dbContext.Notification.FindAsync(id.Value) != null;

    public void Dispose() => _dbContext.Dispose();
}