using System.Collections.Concurrent;
using FCG.IntegrationEvents.V1;
using FCG.Notifications.Api.Notifications;

namespace FCG.Notifications.Tests;

internal sealed class RecordingEmailNotificationSimulator
    : IEmailNotificationSimulator
{
    private readonly ConcurrentQueue<UserCreatedEvent> _welcomeMessages = new();
    private readonly ConcurrentQueue<PaymentProcessedEvent> _purchaseMessages = new();

    public IReadOnlyCollection<UserCreatedEvent> WelcomeMessages =>
        _welcomeMessages.ToArray();

    public IReadOnlyCollection<PaymentProcessedEvent> PurchaseMessages =>
        _purchaseMessages.ToArray();

    public Task SendWelcomeAsync(
        UserCreatedEvent message,
        CancellationToken cancellationToken)
    {
        _welcomeMessages.Enqueue(message);
        return Task.CompletedTask;
    }

    public Task SendPurchaseConfirmationAsync(
        PaymentProcessedEvent message,
        CancellationToken cancellationToken)
    {
        _purchaseMessages.Enqueue(message);
        return Task.CompletedTask;
    }
}
