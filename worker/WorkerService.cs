using inventory_system_api.Application.IService;
using inventory_system_api.Models.Inventory;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class WorkerService : BackgroundService
{

    private IConnection? _connection;
    private IChannel? _channel;
    private IConnectionFactory _factory;
    private IProductService _productService;
    public WorkerService(IConnectionFactory factory, IProductService _service)
    {
        _factory = factory;
        _productService = _service;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Worker waiting 10s for RabbitMQ to stabilize...");
        await Task.Delay(10000, stoppingToken);

        await SetupRabbitMQConsumer(stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task SetupRabbitMQConsumer(CancellationToken stoppingToken)
    {
        int retryCount = 0;
        int maxRetries = 5;

        while (retryCount < maxRetries && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                Console.WriteLine(_factory.UserName);
                Console.WriteLine(_factory.Uri);
                Console.WriteLine(_factory.VirtualHost);

                _connection = await _factory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                await _channel.QueueDeclareAsync(
                    queue: "inventory",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null,
                    cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] WORKER RECEIVED: {message}");

                    await _productService.AddEdit(JsonSerializer.Deserialize<Product>(message));
                };

                await _channel.BasicConsumeAsync(queue: "hello", autoAck: true, consumer: consumer, cancellationToken: stoppingToken);
                Console.WriteLine(" [*] RabbitMQ Consumer is now listening on 'hello' queue.");

                return;
            }
            catch (Exception ex)
            {
                retryCount++;
                Console.WriteLine($"RabbitMQ Setup Attempt {retryCount} failed: {ex.Message}");

                if (retryCount < maxRetries)
                {
                    Console.WriteLine("Retrying in 5 seconds...");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }
    }

  
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null) await _channel.CloseAsync();
        if (_connection != null) await _connection.CloseAsync();
        await base.StopAsync(cancellationToken);
    }
}

public class RabbitMQOptions
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string HostName { get; set; } = string.Empty;

}