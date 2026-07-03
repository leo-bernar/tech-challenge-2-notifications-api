using System.ComponentModel.DataAnnotations;

namespace FCG.Notifications.Api.Configuration;

public sealed class RabbitMqOptions
{
    public const string SectionName = "RabbitMq";

    [Required]
    public string Host { get; init; } = string.Empty;

    [Range(1, 65535)]
    public ushort Port { get; init; } = 5672;

    [Required]
    public string VirtualHost { get; init; } = "/";

    [Required]
    public string Username { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;

    [Required]
    public string UserCreatedQueue { get; init; } = string.Empty;

    [Required]
    public string PaymentProcessedQueue { get; init; } = string.Empty;
}
