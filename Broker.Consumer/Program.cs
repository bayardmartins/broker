using Broker.Consumer.Configuration;
using Broker.Consumer.Infrastructure;
using Broker.Consumer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Broker.Consumer;

internal class Program
{
    public static async Task Main()
    {
        IServiceCollection services = ConfigureServices();
        var serviceProvider = services.BuildServiceProvider();
        await serviceProvider.GetService<IRabbitConsumer>().StartConsumer();
    }

    private static IServiceCollection ConfigureServices()
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
        var config = builder.Build();
        var services = new ServiceCollection();
        services.AddSingleton<IRabbitConsumer, RabbitConsumer>();
        ConfigureDatabase(config, services);

        return services;
    }

    private static void ConfigureDatabase(IConfigurationRoot config, ServiceCollection services)
    {
        var rabbit = config.GetSection("Rabbit").Get<RabbitSettings>();
        services.AddSingleton(rabbit);
        var mongoConfig = config.GetSection("Mongo").Get<MongoSettings>();
        services.AddSingleton(mongoConfig);
        var mongoSetting = MongoClientSettings.FromConnectionString(mongoConfig.Host);
        mongoSetting.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
        services.AddScoped(hc => new HttpClient { BaseAddress = new Uri(rabbit.Url) });
        services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoSetting));
        services.AddSingleton<MongoRepository>();
    }
}