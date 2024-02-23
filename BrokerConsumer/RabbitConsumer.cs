using Broker.Consumer.Interfaces;
using Broker.Domain.Configuration;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using Broker.Domain.Models;
using Broker.Consumer.Infrastructure;
using RabbitMQ.Client;

namespace Broker.Consumer;

public class RabbitConsumer : IRabbitConsumer
{
    private readonly RabbitSettings rabbitSettings;
    private readonly MongoRepository repository;
    private readonly MongoSettings mongoSettings;

    public RabbitConsumer(RabbitSettings settings, MongoRepository repository, MongoSettings mongoSettings)
    {
        this.rabbitSettings = settings;
        this.repository = repository;
        this.mongoSettings = mongoSettings;
    }

    public async Task StartConsumer()
    {
        await ConsumeAsync().ConfigureAwait(false);
        Console.ReadLine();
    }

    private async Task ConsumeAsync()
    {
        var channel = RabbitConfiguration.CreateModel(this.rabbitSettings);
        channel.ExchangeDeclare(exchange: this.rabbitSettings.Exchange, type: ExchangeType.Direct);

        var queueName = this.rabbitSettings.Queue;
        channel.QueueBind(queue: queueName,
                          exchange: this.rabbitSettings.Exchange,
                          routingKey: rabbitSettings.QueueName());
                          //routingKey: string.Empty);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            byte[] body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            this.ConsumeMessageAsync(message);
        };
        channel.BasicConsume(queue: queueName,
                             autoAck: true,
                             consumer: consumer);
    }

    private void ConsumeMessageAsync(string message)
    {
        var rabbitMessage = JsonConvert.DeserializeObject<Message>(message);
        var mongoMessage = rabbitMessage.Map();
        repository.Insert(this.mongoSettings.Database, this.mongoSettings.Collection, mongoMessage);
        Console.WriteLine($"Message stored");
    }
}
