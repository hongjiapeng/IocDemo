using IocDemo.Core.Contracts;
using Microsoft.Extensions.Logging;

namespace IocDemo.Core.Services;

/// <summary>
/// Factory for creating message senders dynamically
/// Allows runtime switching between different sender implementations
/// </summary>
public interface IMessageSenderFactory
{
    IMessageSender CreateSender(MessageSenderType type);
    void SetDefaultSender(MessageSenderType type);
    IMessageSender GetDefaultSender();
}

/// <summary>
/// Types of available message senders
/// </summary>
public enum MessageSenderType
{
    Email,
    Sms
}

/// <summary>
/// Implementation of message sender factory
/// </summary>
public class MessageSenderFactory : IMessageSenderFactory
{
    private readonly ILoggerFactory _loggerFactory;
    private MessageSenderType _defaultType = MessageSenderType.Email;

    public MessageSenderFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public IMessageSender CreateSender(MessageSenderType type)
    {
        return type switch
        {
            MessageSenderType.Email => new EmailSender(_loggerFactory.CreateLogger<EmailSender>()),
            MessageSenderType.Sms => new SmsSender(_loggerFactory.CreateLogger<SmsSender>()),
            _ => throw new ArgumentException($"Unsupported sender type: {type}")
        };
    }

    public void SetDefaultSender(MessageSenderType type)
    {
        _defaultType = type;
    }

    public IMessageSender GetDefaultSender()
    {
        return CreateSender(_defaultType);
    }
}
