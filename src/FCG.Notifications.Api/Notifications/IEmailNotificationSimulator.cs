using FCG.IntegrationEvents.V1;

namespace FCG.Notifications.Api.Notifications;

public interface IEmailNotificationSimulator
{
    Task SendWelcomeAsync(UserCreatedEvent message, CancellationToken cancellationToken);

    Task SendPurchaseConfirmationAsync(
        PaymentProcessedEvent message,
        CancellationToken cancellationToken);
}
