using Microsoft.Extensions.Logging;

namespace Daskata.Core.Services.Error
{
    public class ErrorService : IErrorService
    {
        private readonly ILogger<ErrorService> _logger;

        public ErrorService(ILogger<ErrorService> logger)
        {
            _logger = logger;
        }

        public async Task LogAccessDeniedAsync(Guid? userId)
        {
            if (userId.HasValue)
            {
                _logger.LogInformation($"User with id {userId.Value} tried to access a denied resource.");
            }
            else
            {
                _logger.LogInformation("An anonymous user tried to access a denied resource.");
            }
            await Task.CompletedTask;
        }
    }
}
