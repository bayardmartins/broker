using Broker.Domain.Configuration;
using Broker.Api.Infra;
using MongoDB.Driver;

namespace Broker.Api.Configuration;

public static class AppConfiguration
{
    public static WebApplication BuildWebApplication(this WebApplicationBuilder builder, out RabbitSettings rabbitConfig)
    {
        rabbitConfig = builder.Configuration.GetSection("Rabbit").Get<RabbitSettings>();
        builder.Services.AddSingleton(rabbitConfig);
        var mongoConfig = builder.Configuration.GetSection("Mongo").Get<MongoSettings>();
        builder.Services.AddSingleton(mongoConfig);
        var mongoSetting = MongoClientSettings.FromConnectionString(mongoConfig.Host);
        mongoSetting.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
        var url = rabbitConfig.Url;
        builder.Services.AddScoped(hc => new HttpClient { BaseAddress = new Uri(url) });
        builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoSetting));
        builder.Services.AddSingleton<MongoRepository>();
        var app = builder.Build();
        return app;
    }
}
