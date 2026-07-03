using FCG.Notifications.Api.Configuration;
using FCG.Notifications.Api.Consumers;
using FCG.Notifications.Api.Notifications;
using MassTransit;
using Microsoft.Extensions.Options;

namespace FCG.Notifications.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddNotificationsApi(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddProblemDetails();
        services.AddHealthChecks();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services
            .AddOptions<RabbitMqOptions>()
            .Bind(configuration.GetSection(RabbitMqOptions.SectionName))
            .ValidateDataAnnotations()
            .Validate(options => !string.IsNullOrWhiteSpace(options.VirtualHost),
                "RabbitMq:VirtualHost is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.UserCreatedQueue),
                "RabbitMq:UserCreatedQueue is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.PaymentProcessedQueue),
                "RabbitMq:PaymentProcessedQueue is required.")
            .ValidateOnStart();

        services.AddSingleton<IEmailNotificationSimulator, ConsoleEmailNotificationSimulator>();

        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumer<UserCreatedConsumer>();
            configurator.AddConsumer<PaymentProcessedConsumer>();

            configurator.UsingRabbitMq((context, rabbit) =>
            {
                var options = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

                rabbit.Host(
                    options.Host,
                    options.Port,
                    options.VirtualHost,
                    host =>
                    {
                        host.Username(options.Username);
                        host.Password(options.Password);
                    });

                rabbit.ReceiveEndpoint(options.UserCreatedQueue, endpoint =>
                {
                    endpoint.ConfigureConsumer<UserCreatedConsumer>(context);
                });

                rabbit.ReceiveEndpoint(options.PaymentProcessedQueue, endpoint =>
                {
                    endpoint.ConfigureConsumer<PaymentProcessedConsumer>(context);
                });
            });
        });

        return services;
    }
}
