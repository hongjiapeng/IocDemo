using FluentAssertions;
using IocDemo.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace IocDemo.Core.Tests.Services;

/// <summary>
/// Unit tests for EmailSender
/// Demonstrates how IoC improves testability through dependency injection
/// </summary>
public class EmailSenderTests
{
    private readonly Mock<ILogger<EmailSender>> _loggerMock;
    private readonly EmailSender _emailSender;

    public EmailSenderTests()
    {
        _loggerMock = new Mock<ILogger<EmailSender>>();
        _emailSender = new EmailSender(_loggerMock.Object);
    }

    [Fact]
    public void SenderType_ShouldReturnEmail()
    {
        // Act
        var result = _emailSender.SenderType;

        // Assert
        result.Should().Be("Email");
    }

    [Fact]
    public void Send_WithValidMessage_ShouldReturnFormattedResult()
    {
        // Arrange
        const string message = "Test message";

        // Act
        var result = _emailSender.Send(message);

        // Assert
        result.Should().Be("✉️ Email sent: Test message");
    }

    [Fact]
    public void Send_ShouldLogInformationMessages()
    {
        // Arrange
        const string message = "Test message";

        // Act
        _emailSender.Send(message);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Sending email message")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Email sent successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Theory]
    [InlineData("Short")]
    [InlineData("This is a very long message that contains multiple words and should still work correctly")]
    public void Send_WithVariousMessageLengths_ShouldHandleCorrectly(string message)
    {
        // Act
        var result = _emailSender.Send(message);

        // Assert
        result.Should().StartWith("✉️ Email sent:");
        result.Should().Contain(message);
    }

    [Fact]
    public void Send_WithEmptyString_ShouldHandleCorrectly()
    {
        // Arrange
        const string message = "";

        // Act
        var result = _emailSender.Send(message);

        // Assert
        result.Should().StartWith("✉️ Email sent:");
        result.Should().Be("✉️ Email sent: ");
    }
}
