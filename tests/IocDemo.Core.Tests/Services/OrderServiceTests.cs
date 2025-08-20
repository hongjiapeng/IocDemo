using FluentAssertions;
using IocDemo.Core.Contracts;
using IocDemo.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace IocDemo.Core.Tests.Services;

/// <summary>
/// Unit tests for OrderService
/// Demonstrates testing with multiple dependencies and IoC principles
/// </summary>
public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _repositoryMock;
    private readonly Mock<IMessageSender> _messageSenderMock;
    private readonly Mock<ILogger<OrderService>> _loggerMock;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _repositoryMock = new Mock<IOrderRepository>();
        _messageSenderMock = new Mock<IMessageSender>();
        _loggerMock = new Mock<ILogger<OrderService>>();
        _orderService = new OrderService(_repositoryMock.Object, _messageSenderMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void PlaceOrder_WithValidOrderId_ShouldCallRepositoryAndMessageSender()
    {
        // Arrange
        const string orderId = "ORDER-001";
        const string saveResult = "Order saved";
        const string sendResult = "Message sent";

        _repositoryMock.Setup(r => r.Save(orderId)).Returns(saveResult);
        _messageSenderMock.Setup(m => m.Send($"Order {orderId} processed")).Returns(sendResult);

        // Act
        var result = _orderService.PlaceOrder(orderId);

        // Assert
        result.Should().Be($"{saveResult}\n{sendResult}");
        _repositoryMock.Verify(r => r.Save(orderId), Times.Once);
        _messageSenderMock.Verify(m => m.Send($"Order {orderId} processed"), Times.Once);
    }

    [Fact]
    public void PlaceOrder_WhenRepositoryThrowsException_ShouldReturnErrorMessage()
    {
        // Arrange
        const string orderId = "ORDER-001";
        const string errorMessage = "Database connection failed";
        _repositoryMock.Setup(r => r.Save(orderId)).Throws(new Exception(errorMessage));

        // Act
        var result = _orderService.PlaceOrder(orderId);

        // Assert
        result.Should().StartWith("❌ Failed to process order");
        result.Should().Contain(orderId);
        result.Should().Contain(errorMessage);
    }

    [Fact]
    public void PlaceOrder_WhenMessageSenderThrowsException_ShouldReturnErrorMessage()
    {
        // Arrange
        const string orderId = "ORDER-001";
        const string saveResult = "Order saved";
        const string errorMessage = "Email service unavailable";

        _repositoryMock.Setup(r => r.Save(orderId)).Returns(saveResult);
        _messageSenderMock.Setup(m => m.Send(It.IsAny<string>())).Throws(new Exception(errorMessage));

        // Act
        var result = _orderService.PlaceOrder(orderId);

        // Assert
        result.Should().StartWith("❌ Failed to process order");
        result.Should().Contain(orderId);
        result.Should().Contain(errorMessage);
    }

    [Fact]
    public void GetOrderSummary_ShouldReturnFormattedSummary()
    {
        // Arrange
        var orders = new[] { "ORDER-001", "ORDER-002", "ORDER-003" };
        _repositoryMock.Setup(r => r.GetAllOrders()).Returns(orders);

        // Act
        var result = _orderService.GetOrderSummary();

        // Assert
        result.Should().Be("Currently 3 orders: [ORDER-001, ORDER-002, ORDER-003]");
        _repositoryMock.Verify(r => r.GetAllOrders(), Times.Once);
    }

    [Fact]
    public void GetOrderSummary_WithNoOrders_ShouldReturnEmptySummary()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetAllOrders()).Returns(Array.Empty<string>());

        // Act
        var result = _orderService.GetOrderSummary();

        // Assert
        result.Should().Be("Currently 0 orders: []");
    }

    [Fact]
    public void GetMessageSenderType_ShouldReturnSenderType()
    {
        // Arrange
        const string senderType = "Email";
        _messageSenderMock.Setup(m => m.SenderType).Returns(senderType);

        // Act
        var result = _orderService.GetMessageSenderType();

        // Assert
        result.Should().Be(senderType);
        _messageSenderMock.Verify(m => m.SenderType, Times.Once);
    }

    [Fact]
    public void PlaceOrder_ShouldLogInformationMessages()
    {
        // Arrange
        const string orderId = "ORDER-001";
        _repositoryMock.Setup(r => r.Save(orderId)).Returns("Order saved");
        _messageSenderMock.Setup(m => m.Send(It.IsAny<string>())).Returns("Message sent");

        // Act
        _orderService.PlaceOrder(orderId);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Processing order")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Order processed successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Theory]
    [InlineData("ORDER-001")]
    [InlineData("SIMPLE")]
    [InlineData("VERY-LONG-ORDER-IDENTIFIER")]
    [InlineData("123")]
    public void PlaceOrder_WithDifferentOrderIds_ShouldProcessCorrectly(string orderId)
    {
        // Arrange
        const string saveResult = "Order saved";
        const string sendResult = "Message sent";

        _repositoryMock.Setup(r => r.Save(orderId)).Returns(saveResult);
        _messageSenderMock.Setup(m => m.Send($"Order {orderId} processed")).Returns(sendResult);

        // Act
        var result = _orderService.PlaceOrder(orderId);

        // Assert
        result.Should().Be($"{saveResult}\n{sendResult}");
        _repositoryMock.Verify(r => r.Save(orderId), Times.Once);
        _messageSenderMock.Verify(m => m.Send($"Order {orderId} processed"), Times.Once);
    }
}
