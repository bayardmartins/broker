using BrokerConsumer.Interfaces;

namespace BrokerConsumer;

public class RabbitConsumer : IRabbitConsumer
{
    public async Task StartConsumer()
    {
        await ConsumeAsync().ConfigureAwait(false);
    }

    private async Task ConsumeAsync()
    {
        Console.WriteLine("start");
    }
}
