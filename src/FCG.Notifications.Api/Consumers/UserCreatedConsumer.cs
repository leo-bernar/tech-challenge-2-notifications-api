using FCG.IntegrationEvents.V1;
using FCG.Notifications.Api.Notifications;
using MassTransit;

namespace FCG.Notifications.Api.Consumers;

public sealed class UserCreatedConsumer(
    IEmailNotificationSimulator notifications)
    : IConsumer<UserCreatedEvent>
{
    public Task Consume(ConsumeContext<UserCreatedEvent> context) =>
        ConsumeAsync(context.Message, context.CancellationToken);

    internal Task ConsumeAsync(
        UserCreatedEvent message,
        CancellationToken cancellationToken) =>
        notifications.SendWelcomeAsync(message, cancellationToken);
}
