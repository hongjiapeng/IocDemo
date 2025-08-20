using FluentAssertions;
using IocDemo.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace IocDemo.Core.Tests.Services;

/// <summary>
/// Unit tests for InMemoryOrderRepository
/// Demonstrates repository pattern testing
/// </summary>
public class InMemoryOrderRepositoryTests
{
    private readonly Mock<ILogger<InMemoryOrderRepository>> _loggerMock;
    private readonly InMemoryOrderRepository _repository;

    public InMemoryOrderRepositoryTests()
    {
        _loggerMock = new Mock<ILogger<InMemoryOrderRepository>>();
        _repository = new InMemoryOrderRepository(_loggerMock.Object);
    }

    [Fact]
    public void OrderCount_InitiallyEmpty_ShouldReturnZero()
    {
        // Act & Assert
        _repository.OrderCount.Should().Be(0);
    }

    [Fact]
    public void Save_WithValidOrderId_ShouldIncreaseCount()
    {
        // Arrange
        const string orderId = "ORDER-001";

        // Act
        _repository.Save(orderId);

        // Assert
        _repository.OrderCount.Should().Be(1);
    }

    [Fact]
    public void Save_WithValidOrderId_ShouldReturnFormattedMessage()
    {
        // Arrange
        const string orderId = "ORDER-001";

        // Act
        var result = _repository.Save(orderId);

        // Assert
        result.Should().Be("💾 Order ORDER-001 saved to repository");
    }

    [Fact]
    public void GetAllOrders_WithNoOrders_ShouldReturnEmptyArray()
    {
        // Act
        var result = _repository.GetAllOrders();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetAllOrders_WithMultipleOrders_ShouldReturnAllOrders()
    {
        // Arrange
        const string orderId1 = "ORDER-001";
        const string orderId2 = "ORDER-002";
        const string orderId3 = "ORDER-003";

        // Act
        _repository.Save(orderId1);
        _repository.Save(orderId2);
        _repository.Save(orderId3);
        var result = _repository.GetAllOrders();

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(orderId1);
        result.Should().Contain(orderId2);
        result.Should().Contain(orderId3);
    }

    [Fact]
    public void Save_ShouldLogInformationMessages()
    {
        // Arrange
        const string orderId = "ORDER-001";

        // Act
        _repository.Save(orderId);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Saving order")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Order saved successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Theory]
    [InlineData("ORDER-001")]
    [InlineData("SIMPLE")]
    [InlineData("VERY-LONG-ORDER-IDENTIFIER-WITH-MANY-CHARACTERS")]
    [InlineData("123")]
    public void Save_WithDifferentOrderIdFormats_ShouldWorkCorrectly(string orderId)
    {
        // Act
        var result = _repository.Save(orderId);

        // Assert
        result.Should().Contain(orderId);
        _repository.OrderCount.Should().Be(1);
        _repository.GetAllOrders().Should().Contain(orderId);
    }
}
