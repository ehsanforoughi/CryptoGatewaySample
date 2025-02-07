using Dapper;
using System.Data.Common;
using CryptoGateway.Service.Models.Notification;

namespace CryptoGateway.Service.Contracts.Query;

public static class NotificationQueries
{
    public static async Task<IEnumerable<NotificationReadModels.NotificationListItem>> Query(
        this DbConnection connection, NotificationQueryModels.GetNotSentNotifications query) =>
        await connection.QueryAsync<NotificationReadModels.NotificationListItem>(
            @"SELECT [NotificationId]
	              ,u.[UserExternalId] AS [UserId]
	              ,u.[FirstName]
	              ,u.[LastName]
                  ,[Type] AS [NotifType]
                  ,[ActionType] AS [AType]
                  ,[PriorityType] AS [PType]
                  ,[TryCount]
                  ,[IsSent]
                  ,[IsSuccess]
	              ,FORMAT(n.[SentAt], 'yyyy-MM-dd hh:mm') AS [SentAt]
                  ,[SendStatus]
                  ,[Receiver]
                  ,[Text]
	              ,FORMAT(n.[CreatedAt], 'yyyy-MM-dd hh:mm') AS [CreatedAt]
                  ,FORMAT(n.[ModifiedAt], 'yyyy-MM-dd hh:mm') AS [ModifiedAt]
              FROM [Messaging].[Notification] n INNER JOIN
	                   [Base].[User] u ON n.UserId = u.UserId
              WHERE n.[IsDeleted] = 0 AND n.[IsSent] = 0 AND 
                        n.[CreatedAt] > DateADD(MINUTE, -20, GETDATE()) AND n.[TryCount] < @TryCount
              ORDER BY [NotificationId]",
            new
            {
                TryCount = query.TryCount
            });
}