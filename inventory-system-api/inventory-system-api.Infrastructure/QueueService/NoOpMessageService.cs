using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using inventory_system_api.Application.IQueue;

namespace inventory_system_api.Infrastructure.QueueService
{
    public class NoOpMessageService : IMessageService
    {
        private readonly ILogger<NoOpMessageService> _logger;
        public NoOpMessageService(ILogger<NoOpMessageService> logger)
        {
            _logger = logger;
        }

        public Task<bool> Enqueue(string message)
        {
            _logger.LogDebug("Messaging disabled - NoOpMessageService skipped publish. Message: {Message}", message);
            return Task.FromResult(true);
        }
    }
}
