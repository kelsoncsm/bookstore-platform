namespace BookStore.BuildingBlocks.Contracts.Notifications;

public sealed record NotificationDto(Guid Id, string Type, string Message, DateTime CreatedAtUtc);

public sealed record CreateNotificationRequest(string Type, string Message);
