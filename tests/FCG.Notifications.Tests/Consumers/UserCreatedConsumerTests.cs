using FCG.IntegrationEvents.V1;
using FCG.Notifications.Api.Consumers;

namespace FCG.Notifications.Tests.Consumers;

public sealed class UserCreatedConsumerTests
{
    [Fact]
    public async Task Consume_SendsWelcomeNotification()
    {
        var recorder = new RecordingEmailNotificationSimulator();
        var consumer = new UserCreatedConsumer(recorder);

        var message = new UserCreatedEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Guid.NewGuid(),
            "Ada Lovelace",
            "ada@example.com");

        await consumer.ConsumeAsync(message, CancellationToken.None);

        Assert.Equal(message, Assert.Single(recorder.WelcomeMessages));
    }
}
