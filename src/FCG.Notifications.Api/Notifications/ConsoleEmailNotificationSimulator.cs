using FCG.IntegrationEvents.V1;

namespace FCG.Notifications.Api.Notifications;

public sealed class ConsoleEmailNotificationSimulator(
    ILogger<ConsoleEmailNotificationSimulator> logger)
    : IEmailNotificationSimulator
{
    public Task SendWelcomeAsync(
        UserCreatedEvent message,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Welcome email simulated. EventId: {EventId}, UserId: {UserId}, Name: {Name}, Email: {Email}",
            message.EventId,
            message.UserId,
            message.Name,
            message.Email);

        return Task.CompletedTask;
    }

    public Task SendPurchaseConfirmationAsync(
        PaymentProcessedEvent message,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Purchase confirmation email simulated. OrderId: {OrderId}, UserId: {UserId}, Email: {Email}, GameId: {GameId}, Price: {Price}",
            message.OrderId,
            message.UserId,
            message.UserEmail,
            message.GameId,
            message.Price);

        return Task.CompletedTask;
    }
}
