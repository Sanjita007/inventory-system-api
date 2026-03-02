using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<IConnectionFactory>(sp =>
{
    var config = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQOptions>();
    return new ConnectionFactory()
    {
        HostName = config.HostName,
        Port = config.Port,
        UserName = config.UserName,
        Password = config.Password
    };

});

Console.WriteLine("connecting to rabbit mq");

builder.Services.AddHostedService<WorkerService>();

var host = builder.Build();
await host.RunAsync();