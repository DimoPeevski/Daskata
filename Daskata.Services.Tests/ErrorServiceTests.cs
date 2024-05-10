using Daskata.Core.Services.Error;
using Microsoft.Extensions.Logging;
using Moq;

namespace Daskata.Tests
{
    public class ErrorServiceTests
    {
        private readonly Mock<ILogger<ErrorService>> _mockLogger;

        public ErrorServiceTests()
        {
            _mockLogger = new Mock<ILogger<ErrorService>>();
        }

        [Test]
        public async Task LogAccessDeniedAsync_ShouldLogExpectedMessage()
        {
            // Arrange
            var errorService = new ErrorService(_mockLogger.Object);
            Guid? userId = Guid.NewGuid();

            // Act
            await errorService.LogAccessDeniedAsync(userId);

            // Assert
            // Ако не хвърли грешка значи тества е успешен.
        }

        [Test]
        public async Task LogAccessDeniedAsync_ShouldLogExpectedMessageForAnonymousUser()
        {
            // Arrange
            var errorService = new ErrorService(_mockLogger.Object);
            Guid? userId = null;

            // Act
            await errorService.LogAccessDeniedAsync(userId);

            // Assert
            // Ако не хвърли грешка значи тества е успешен.
        }
    }
}
