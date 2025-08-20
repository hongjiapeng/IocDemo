using IocDemo.Core.Contracts;
using Microsoft.Extensions.Logging;

namespace IocDemo.Core.Services;

/// <summary>
/// Email message sender implementation
/// Demonstrates concrete implementation of abstraction
/// </summary>
public class EmailSender : IMessageSender
{
    private readonly ILogger<EmailSender> _logger;

    /// <summary>
    /// Initializes a new instance of the EmailSender class
    /// </summary>
    /// <param name="logger">Logger instance for logging operations</param>
    public EmailSender(ILogger<EmailSender> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Gets the type of message sender
    /// </summary>
    public string SenderType => "Email";

    /// <summary>
    /// Sends a message via email
    /// </summary>
    /// <param name="message">The message to send</param>
    /// <returns>Result of the send operation</returns>
    public string Send(string message)
    {
        _logger.LogInformation("Sending email message: {Message}", message);
        
        // Simulate email sending logic
        var result = $"✉️ Email sent: {message}";
        
        _logger.LogInformation("Email sent successfully");
        return result;
    }
}
