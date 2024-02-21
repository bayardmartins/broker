using BrokerApi.Configuration;
using BrokerApi.Infra;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var rabbitConfig = builder.Configuration.GetSection("Rabbit").Get<Rabbit>();
builder.Services.AddSingleton(rabbitConfig);
var mongoConfig = builder.Configuration.GetSection("Mongo").Get<Mongo>();
builder.Services.AddSingleton(mongoConfig);
var mongoSetting = MongoClientSettings.FromConnectionString(mongoConfig.Host);
mongoSetting.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
builder.Services.AddScoped(hc => new HttpClient {  BaseAddress = new Uri(rabbitConfig.Url) });
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoSetting));
builder.Services.AddSingleton<MongoRepository>();
var app = builder.Build();

RabbitConfiguration.Config(rabbitConfig);

app.UseHttpsRedirection();

app.MapPost("/message", ([FromBody]Message message, [FromServices] Rabbit rabbitConfig) =>
{
    using StringContent jsonContent = new(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

    var model = RabbitConfiguration.CreateModel(rabbitConfig);
    var props = model.CreateBasicProperties();
    props.Persistent = false;
    byte[] msgBuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(message));
    model.BasicPublish(rabbitConfig.Exchange, rabbitConfig.QueueName(), props, msgBuffer);
});

app.MapGet("/messages", async ([FromServices] Mongo mongoConfig, [FromServices] MongoRepository repository) =>
{
    var messages = await repository.FindMany(mongoConfig.Database, mongoConfig.Collection);
    return messages.Select(x => x.Map());
});

app.Run();