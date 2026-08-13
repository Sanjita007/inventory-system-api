using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace inventory_system_api.integrationTests.TestHost;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    //private SqliteConnection? _connection;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("TestConnectionString");
            
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDbConnection));
            if (descriptor != null) services.Remove(descriptor);

            services.AddTransient<IDbConnection>(sp =>
            {
                var conn = new SqlConnection(connectionString);
                return conn;
            });
            // register a test authentication scheme
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

            // set the test scheme as default
            services.PostConfigure<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";

            });
        });
    }

    //protected override void Dispose(bool disposing)
    //{
    //    base.Dispose(disposing);
    //    if (_connection != null)
    //    {
    //        _connection.Close();
    //        _connection.Dispose();
    //        _connection = null;
    //    }
    //}
}
