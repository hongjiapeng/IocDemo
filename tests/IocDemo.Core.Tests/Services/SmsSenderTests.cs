using FluentAssertions;
using IocDemo.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace IocDemo.Core.Tests.Services;

/// <summary>
/// Unit tests for SmsSender
/// Demonstrates alternative implementation testing
/// </summary>
public class SmsSenderTests
{
    private readonly Mock<ILogger<SmsSender>> _loggerMock;
    private readonly SmsSender _smsSender;

    public SmsSenderTests()
    {
        _loggerMock = new Mock<ILogger<SmsSender>>();
        _smsSender = new SmsSender(_loggerMock.Object);
    }

    [Fact]
    public void SenderType_ShouldReturnSMS()
    {
        // Act
        var result = _smsSender.SenderType;

        // Assert
        result.Should().Be("SMS");
    }

    [Fact]
    public void Send_WithValidMessage_ShouldReturnFormattedResult()
    {
        // Arrange
        const string message = "Test SMS message";

        // Act
        var result = _smsSender.Send(message);

        // Assert
        result.Should().Be("📱 SMS sent: Test SMS message");
    }

    [Fact]
    public void Send_ShouldLogInformationMessages()
    {
        // Arrange
        const string message = "Test message";

        // Act
        _smsSender.Send(message);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Sending SMS message")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("SMS sent successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
