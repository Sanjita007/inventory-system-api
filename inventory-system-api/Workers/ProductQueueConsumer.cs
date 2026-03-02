using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using inventory_system_api.Application.IQueue;
using inventory_system_api.Application.IService;

namespace inventory_system_api.Workers
{
    // Example background worker that consumes messages (or polls) and uses application services.
    // In a real setup this would be in a separate Worker project that references Application+Infrastructure.
    public class ProductQueueConsumer : BackgroundService
    {
        private readonly ILogger<ProductQueueConsumer> _logger;
        private readonly IMessageService _messageService;
        private readonly IProductService _productService;

        public ProductQueueConsumer(ILogger<ProductQueueConsumer> logger, IMessageService messageService, IProductService productService)
        {
            _logger = logger;
            _messageService = messageService;
            _productService = productService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ProductQueueConsumer started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // This is a placeholder loop. Replace with real message consumption.
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

                    // Example: call service to do some periodic work
                    // await _productService.DoPeriodicWork();

                    // Example: publish a heartbeat (no-op when messaging disabled)
                    await _messageService.Enqueue(JsonSerializer.Serialize(new { Event = "Heartbeat", Timestamp = DateTime.UtcNow }));
                }
                catch (TaskCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    // shutting down
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in ProductQueueConsumer loop");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }

            _logger.LogInformation("ProductQueueConsumer stopping");
        }
    }
}
