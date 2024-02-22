using RabbitMQ.Client;

namespace BrokerApi.Configuration
{
    public static class RabbitConfiguration
    {
        public static void Config(RabbitSettings rabbitConfig)
        {
            var model = CreateModel(rabbitConfig);
            model.ExchangeDeclare(rabbitConfig.Exchange, ExchangeType.Direct);
            model.QueueDeclare(rabbitConfig.Queue, true, false, false, null);
            model.QueueBind(rabbitConfig.Queue, rabbitConfig.Exchange, rabbitConfig.QueueName());
        }

        public static IModel CreateModel(RabbitSettings rabbitConfig)
        {
            var connectionFactory = new ConnectionFactory()
            {
                UserName = rabbitConfig.UserName,
                Password = rabbitConfig.Password,
                HostName = rabbitConfig.Url,
            };

            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();
            return model;
        }
    }
}
