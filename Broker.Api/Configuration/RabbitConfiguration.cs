using RabbitMQ.Client;

namespace Broker.Api.Configuration
{
    public static class RabbitConfiguration
    {
        public static IModel Config(RabbitSettings rabbitSettings)
        {
            var model = CreateModel(rabbitSettings);
            model.ExchangeDeclare(rabbitSettings.Exchange, ExchangeType.Direct);
            model.QueueDeclare(rabbitSettings.Queue, true, false, false, null);
            model.QueueBind(rabbitSettings.Queue, rabbitSettings.Exchange, rabbitSettings.QueueName());
            return model;
        }

        public static IModel CreateModel(RabbitSettings rabbitSettings)
        {
            var connectionFactory = new ConnectionFactory()
            {
                UserName = rabbitSettings.UserName,
                Password = rabbitSettings.Password,
                HostName = rabbitSettings.Url,
            };

            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();
            return model;
        }
    }
}
