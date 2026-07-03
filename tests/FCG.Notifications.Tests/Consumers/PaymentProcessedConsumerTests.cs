using FCG.IntegrationEvents.V1;
using FCG.Notifications.Api.Consumers;
using Microsoft.Extensions.Logging.Abstractions;

namespace FCG.Notifications.Tests.Consumers;

public sealed class PaymentProcessedConsumerTests
{
    [Fact]
    public async Task Consume_WhenApproved_SendsPurchaseConfirmation()
    {
        var recorder = new RecordingEmailNotificationSimulator();
        var consumer = CreateConsumer(recorder);

        var message = CreateMessage(PaymentStatuses.Approved);
        await consumer.ConsumeAsync(message, CancellationToken.None);

        Assert.Equal(message, Assert.Single(recorder.PurchaseMessages));
    }

    [Fact]
    public async Task Consume_WhenRejected_DoesNotSendPurchaseConfirmation()
    {
        var recorder = new RecordingEmailNotificationSimulator();
        var consumer = CreateConsumer(recorder);

        await consumer.ConsumeAsync(
            CreateMessage(PaymentStatuses.Rejected),
            CancellationToken.None);

        Assert.Empty(recorder.PurchaseMessages);
    }

    private static PaymentProcessedConsumer CreateConsumer(
        RecordingEmailNotificationSimulator recorder) =>
        new(
            recorder,
            NullLogger<PaymentProcessedConsumer>.Instance);

    private static PaymentProcessedEvent CreateMessage(string status) =>
        new(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Guid.NewGuid(),
            "ada@example.com",
            Guid.NewGuid(),
            59.90m,
            status);
}
