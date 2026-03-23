using inventory_system_api.Application.IRepository;
using inventory_system_api.Infrastructure.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<IDbConnection>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnectionString");

    Console.WriteLine($"DEBUG: Worker Connection String is: {connectionString}");

    if (string.IsNullOrEmpty(connectionString))
        throw new Exception("Connection string is NULL! Check appsettings.json or Docker ENV.");

    return new SqlConnection(connectionString);
});

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
builder.Services.AddTransient<ITaxRepository, TaxRepository>();  
builder.Services.AddHostedService<WorkerService>();

var host = builder.Build();
await host.RunAsync();