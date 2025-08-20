# Configuration Examples for IoC Demo

This document shows how to easily switch between different implementations using the IoC container.

## Switching Message Providers

### Using Email Sender (Default)

In `Program.cs` or `App.xaml.cs`:

```csharp
services.AddIocDemoCore(useEmailSender: true);
// OR simply
services.AddIocDemoCore(); // Email is default
```

### Using SMS Sender

```csharp
services.AddIocDemoCore(useEmailSender: false);
// OR
services.AddIocDemoCoreWithSms();
```

## Custom Configuration Example

You can also register services manually for more control:

```csharp
// Manual registration example
services.AddSingleton<IMessageSender, EmailSender>();
services.AddScoped<IOrderRepository, InMemoryOrderRepository>();
services.AddTransient<OrderService>();

// Or for SMS
services.AddSingleton<IMessageSender, SmsSender>();
services.AddScoped<IOrderRepository, InMemoryOrderRepository>();
services.AddTransient<OrderService>();
```

## Creating Custom Implementations

### Custom Message Sender

```csharp
public class SlackSender : IMessageSender
{
    private readonly ILogger<SlackSender> _logger;

    public SlackSender(ILogger<SlackSender> logger)
    {
        _logger = logger;
    }

    public string SenderType => "Slack";

    public string Send(string message)
    {
        _logger.LogInformation("Sending Slack message: {Message}", message);
        return $"💬 Slack message sent: {message}";
    }
}
```

### Register Custom Implementation

```csharp
services.AddSingleton<IMessageSender, SlackSender>();
```

## Benefits Demonstrated

1. **Zero Code Changes**: Switch implementations without modifying business logic
2. **Configuration-Driven**: Behavior changes through DI registration
3. **Easy Testing**: Replace with mock implementations
4. **Extensibility**: Add new implementations without breaking existing code

## Testing with Different Configurations

```csharp
[Fact]
public void OrderService_WithEmailSender_ShouldUseEmail()
{
    // Arrange
    var services = new ServiceCollection();
    services.AddIocDemoCore(useEmailSender: true);
    using var provider = services.BuildServiceProvider();
    
    // Act
    var orderService = provider.GetRequiredService<OrderService>();
    var senderType = orderService.GetMessageSenderType();
    
    // Assert
    senderType.Should().Be("Email");
}

[Fact]
public void OrderService_WithSmsSender_ShouldUseSms()
{
    // Arrange
    var services = new ServiceCollection();
    services.AddIocDemoCoreWithSms();
    using var provider = services.BuildServiceProvider();
    
    // Act
    var orderService = provider.GetRequiredService<OrderService>();
    var senderType = orderService.GetMessageSenderType();
    
    // Assert
    senderType.Should().Be("SMS");
}
```
