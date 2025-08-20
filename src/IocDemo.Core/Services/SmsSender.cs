using IocDemo.Core.Contracts;
using Microsoft.Extensions.Logging;

namespace IocDemo.Core.Services;

/// <summary>
/// SMS message sender implementation
/// Demonstrates alternative implementation of the same interface
/// </summary>
public class SmsSender : IMessageSender
{
    private readonly ILogger<SmsSender> _logger;

    /// <summary>
    /// Initializes a new instance of the SmsSender class
    /// </summary>
    /// <param name="logger">Logger instance for logging operations</param>
    public SmsSender(ILogger<SmsSender> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Gets the type of message sender
    /// </summary>
    public string SenderType => "SMS";

    /// <summary>
    /// Sends a message via SMS
    /// </summary>
    /// <param name="message">The message to send</param>
    /// <returns>Result of the send operation</returns>
    public string Send(string message)
    {
        _logger.LogInformation("Sending SMS message: {Message}", message);
        
        // Simulate SMS sending logic
        var result = $"📱 SMS sent: {message}";
        
        _logger.LogInformation("SMS sent successfully");
        return result;
    }
}
