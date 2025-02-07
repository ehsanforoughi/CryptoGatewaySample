using Dapper;
using Quartz;
using Bat.Core;
using System.Data.Common;
using CryptoGateway.Framework;
using Microsoft.Extensions.Configuration;
using CryptoGateway.Service.Contracts.Query;
using Microsoft.Extensions.DependencyInjection;
using CryptoGateway.Service.Models.Notification;
using CryptoGateway.Domain.Entities.Notification;
using CryptoGateway.Service.Strategies.Notification.Sender;
using CryptoGateway.Domain.Entities.Notification.ValueObjects;

namespace CryptoGateway.Service.Quartz.Jobs.Messaging;

[DisallowConcurrentExecution]
public class SendingNotificationJob : IJob, IDisposable
{
    private readonly IServiceProvider _serviceProvider;

    public SendingNotificationJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            FileLoger.Message($"SendingNotificationJob is started: {DateTime.Now.ToString("HH:mm:ss")}");
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            var configuration = scope.ServiceProvider.GetService<IConfiguration>();
            var connection = scope.ServiceProvider.GetService<DbConnection>();
            var notificationRepository = scope.ServiceProvider.GetService<INotificationRepository>();
            var tryCount = int.Parse(configuration["QuartzSettings:SendNotificationTryCount"]!);
            //var fetchCount = int.Parse(_configuration["QuartzSettings:SendNotificationFetchCount"]!);

            var request = new NotificationQueryModels.GetNotSentNotifications
            {
                TryCount = tryCount
            };

            var notSentList = await connection.Query(request);
            foreach (var notSent in notSentList.ToList())
            {
                var notification = await notificationRepository.Load(new NotificationId(notSent.NotificationId));
                if (notification != null)
                    NotificationSenderFactory.GetStrategy(notSent.NotificationType)
                        .Send(notification, unitOfWork, configuration);
            }
            FileLoger.Message($"SendingNotificationJob is ended: {DateTime.Now.ToString("HH:mm:ss")}");
        }
        catch (Exception e)
        {
            FileLoger.CriticalError(e);
        }
    }

    public void Dispose() => GC.SuppressFinalize(this);
}