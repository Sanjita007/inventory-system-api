using inventory_system_api.Application.IQueue;
using RabbitMQ.Client;
using System.Data;
using System.Text;

namespace inventory_system_api.Infrastructure.QueueService
{

    public class MessageService : IMessageService
    {
        private readonly IConnectionFactory _factory;
        private IConnection? _conn;
        private IChannel? _channel;

        public MessageService(IConnectionFactory factory)
        {
            _factory = factory;
        }

        private async Task EnsureConnected()
        {
            // Only connect if the connection/channel is closed or null
            if (_conn == null || !_conn.IsOpen || _channel == null || !_channel.IsOpen)
            {
                Console.WriteLine(" [!] Connecting to RabbitMQ (inventory-rabbit)...");
                _conn = await _factory.CreateConnectionAsync();
                _channel = await _conn.CreateChannelAsync();

                await _channel.QueueDeclareAsync(
                    queue: "inventory",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
            }
        }

        public async Task<bool> Enqueue(string messageString)
        {
            try
            {
                await EnsureConnected();

                var body = Encoding.UTF8.GetBytes(messageString);

                // In v7+, you don't need to specify <BasicProperties> if you aren't using them
                await _channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: "inventory",
                    mandatory: true,
                    body: body);

                Console.WriteLine($" [x] API Published to RabbitMQ: {messageString}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" [!] API Failed to Publish: {ex.Message}");
                return false;
            }
        }
    }
}