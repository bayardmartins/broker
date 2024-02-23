using Broker.Domain.Configuration;
using Broker.Api.Configuration;
using Broker.Api.Infra;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Broker.Domain.Models;

Console.WriteLine("Start");

var builder = WebApplication.CreateBuilder(args);
var app = builder.BuildWebApplication(out var rabbitSettings);
RabbitConfiguration.Config(rabbitSettings);

app.UseHttpsRedirection();

app.MapPost("/message", ([FromBody]Message message, [FromServices] RabbitSettings rabbitSettings) =>
{
    using StringContent jsonContent = new(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

    var model = RabbitConfiguration.CreateModel(rabbitSettings);
    var props = model.CreateBasicProperties();
    props.Persistent = false;
    byte[] msgBuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(message));
    model.BasicPublish(rabbitSettings.Exchange, rabbitSettings.QueueName(), props, msgBuffer);
});

app.MapGet("/messages", async ([FromServices] MongoSettings mongoSettings, [FromServices] MongoRepository repository) =>
{
    var messages = await repository.FindMany(mongoSettings.Database, mongoSettings.Collection);
    return messages.Select(x => x.Map());
});

Console.WriteLine("Prepare to Run");

app.Run();

Console.WriteLine("Running");