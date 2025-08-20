using FluentAssertions;
using IocDemo.Core;
using IocDemo.Core.Contracts;
using IocDemo.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IocDemo.Core.Tests.Integration;

/// <summary>
/// Integration tests for dependency injection configuration
/// Demonstrates how IoC container resolves dependencies
/// </summary>
public class DependencyInjectionIntegrationTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public DependencyInjectionIntegrationTests()
    {
        var services = new ServiceCollection();
        services.AddIocDemoCore()
                .AddEmailSender(); // Use email sender for testing
        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public void ServiceProvider_ShouldResolveOrderService()
    {
        // Act
        var orderService = _serviceProvider.GetService<OrderService>();

        // Assert
        orderService.Should().NotBeNull();
    }

    [Fact]
    public void ServiceProvider_ShouldResolveIMessageSenderAsEmailSender()
    {
        // Act
        var messageSender = _serviceProvider.GetService<IMessageSender>();

        // Assert
        messageSender.Should().NotBeNull();
        messageSender.Should().BeOfType<EmailSender>();
        messageSender!.SenderType.Should().Be("Email");
    }

    [Fact]
    public void ServiceProvider_ShouldResolveIOrderRepository()
    {
        // Act
        var repository = _serviceProvider.GetService<IOrderRepository>();

        // Assert
        repository.Should().NotBeNull();
        repository.Should().BeOfType<InMemoryOrderRepository>();
    }

    [Fact]
    public void OrderService_ShouldWorkWithResolvedDependencies()
    {
        // Arrange
        var orderService = _serviceProvider.GetRequiredService<OrderService>();

        // Act
        var result = orderService.PlaceOrder("INTEGRATION-TEST-001");

        // Assert
        result.Should().Contain("💾 Order INTEGRATION-TEST-001 saved to repository");
        result.Should().Contain("✉️ Email sent: Order INTEGRATION-TEST-001 processed");
    }

    [Fact]
    public void OrderService_ShouldMaintainOrderState()
    {
        // Arrange
        var orderService = _serviceProvider.GetRequiredService<OrderService>();

        // Act
        orderService.PlaceOrder("ORDER-001");
        orderService.PlaceOrder("ORDER-002");
        var summary = orderService.GetOrderSummary();

        // Assert
        summary.Should().Contain("Currently 2 orders:");
        summary.Should().Contain("ORDER-001");
        summary.Should().Contain("ORDER-002");
    }

    [Fact]
    public void ServiceProvider_WithSmsConfiguration_ShouldResolveSmsSender()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddIocDemoCoreWithSms();
        using var smsServiceProvider = services.BuildServiceProvider();

        // Act
        var messageSender = smsServiceProvider.GetService<IMessageSender>();

        // Assert
        messageSender.Should().NotBeNull();
        messageSender.Should().BeOfType<SmsSender>();
        messageSender!.SenderType.Should().Be("SMS");
    }

    [Fact]
    public void OrderService_WithSmsConfiguration_ShouldUseSms()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddIocDemoCoreWithSms();
        using var smsServiceProvider = services.BuildServiceProvider();
        var orderService = smsServiceProvider.GetRequiredService<OrderService>();

        // Act
        var result = orderService.PlaceOrder("SMS-TEST-001");
        var senderType = orderService.GetMessageSenderType();

        // Assert
        result.Should().Contain("📱 SMS sent: Order SMS-TEST-001 processed");
        senderType.Should().Be("SMS");
    }

    [Fact]
    public void ServiceProvider_ShouldCreateNewTransientInstances()
    {
        // Act
        var orderService1 = _serviceProvider.GetService<OrderService>();
        var orderService2 = _serviceProvider.GetService<OrderService>();

        // Assert
        orderService1.Should().NotBeNull();
        orderService2.Should().NotBeNull();
        orderService1.Should().NotBeSameAs(orderService2);
    }

    [Fact]
    public void ServiceProvider_ShouldReturnSameSingletonInstance()
    {
        // Act
        var messageSender1 = _serviceProvider.GetService<IMessageSender>();
        var messageSender2 = _serviceProvider.GetService<IMessageSender>();

        // Assert
        messageSender1.Should().NotBeNull();
        messageSender2.Should().NotBeNull();
        messageSender1.Should().BeSameAs(messageSender2);
    }

    public void Dispose()
    {
        _serviceProvider?.Dispose();
    }
}
