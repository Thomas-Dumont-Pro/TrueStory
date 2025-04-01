using Application.Common.Behaviours;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.Common;

public class UnhandledExceptionBehaviourTests
{
    private readonly Mock<ILogger<UnhandledExceptionBehaviour<object, object>>> _logger;
    private readonly UnhandledExceptionBehaviour<object, object> _behaviour;
    private readonly Mock<RequestHandlerDelegate<object>> _next;

    public UnhandledExceptionBehaviourTests()
    {
        _logger = new Mock<ILogger<UnhandledExceptionBehaviour<object, object>>>();
        _next = new Mock<RequestHandlerDelegate<object>>();
        _behaviour = new UnhandledExceptionBehaviour<object, object>(_logger.Object);
    }

    [Test]
    public async Task Handle_ShouldLogError_WhenExceptionIsThrown()
    {
        // Arrange
        var request = new object();
        var exception = new Exception("Test exception");
        _next.Setup(n => n()).ThrowsAsync(exception);

        // Act & Assert
        var result = await _behaviour.Invoking(b => b.Handle(request, _next.Object, new CancellationToken()))
                                     .Should().ThrowAsync<Exception>();

        _logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Unhandled Exception")),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    [Test]
    public async Task Handle_ShouldCallNextDelegate_WhenNoExceptionIsThrown()
    {
        // Arrange
        var request = new object();
        _next.Setup(n => n()).ReturnsAsync(new object());

        // Act
        var result = await _behaviour.Handle(request, _next.Object, new CancellationToken());

        // Assert
        result.Should().NotBeNull();
        _next.Verify(n => n(), Times.Once);
    }
}
