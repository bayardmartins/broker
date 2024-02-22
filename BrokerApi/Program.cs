using BrokerApi.Configuration;
using BrokerApi.Infra;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.BuildWebApplication(out var rabbitConfig);
RabbitConfiguration.Config(rabbitConfig);

app.UseHttpsRedirection();

app.MapPost("/message", ([FromBody]Message message, [FromServices] RabbitSettings rabbitConfig) =>
{
    using StringContent jsonContent = new(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

    var model = RabbitConfiguration.CreateModel(rabbitConfig);
    var props = model.CreateBasicProperties();
    props.Persistent = false;
    byte[] msgBuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(message));
    model.BasicPublish(rabbitConfig.Exchange, rabbitConfig.QueueName(), props, msgBuffer);
});

app.MapGet("/messages", async ([FromServices] MongoSettings mongoConfig, [FromServices] MongoRepository repository) =>
{
    var messages = await repository.FindMany(mongoConfig.Database, mongoConfig.Collection);
    return messages.Select(x => x.Map());
});

app.Run();