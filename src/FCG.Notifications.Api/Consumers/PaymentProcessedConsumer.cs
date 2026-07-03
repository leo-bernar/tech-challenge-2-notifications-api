using FCG.IntegrationEvents.V1;
using FCG.Notifications.Api.Notifications;
using MassTransit;

namespace FCG.Notifications.Api.Consumers;

public sealed class PaymentProcessedConsumer(
    IEmailNotificationSimulator notifications,
    ILogger<PaymentProcessedConsumer> logger)
    : IConsumer<PaymentProcessedEvent>
{
    public Task Consume(ConsumeContext<PaymentProcessedEvent> context) =>
        ConsumeAsync(context.Message, context.CancellationToken);

    internal Task ConsumeAsync(
        PaymentProcessedEvent message,
        CancellationToken cancellationToken)
    {
        if (string.Equals(
                message.Status,
                PaymentStatuses.Approved,
                StringComparison.OrdinalIgnoreCase))
        {
            return notifications.SendPurchaseConfirmationAsync(
                message,
                cancellationToken);
        }

        if (!string.Equals(
                message.Status,
                PaymentStatuses.Rejected,
                StringComparison.OrdinalIgnoreCase))
        {
            logger.LogWarning(
                "Payment result has unknown status. OrderId: {OrderId}, Status: {Status}",
                message.OrderId,
                message.Status);

            return Task.CompletedTask;
        }

        logger.LogInformation(
            "Purchase confirmation skipped for rejected payment. OrderId: {OrderId}, UserId: {UserId}, GameId: {GameId}",
            message.OrderId,
            message.UserId,
            message.GameId);

        return Task.CompletedTask;
    }
}
