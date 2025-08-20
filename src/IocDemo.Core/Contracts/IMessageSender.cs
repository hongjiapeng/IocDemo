namespace IocDemo.Core.Contracts;

/// <summary>
/// Contract for message sending functionality
/// Demonstrates dependency inversion principle - high-level modules depend on abstractions
/// </summary>
public interface IMessageSender
{
    /// <summary>
    /// Sends a message using the specific implementation
    /// </summary>
    /// <param name="message">The message to send</param>
    /// <returns>Result of the send operation</returns>
    string Send(string message);
    
    /// <summary>
    /// Gets the type of message sender
    /// </summary>
    string SenderType { get; }
}
