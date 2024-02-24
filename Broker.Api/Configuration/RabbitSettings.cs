namespace Broker.Api.Configuration;

public class RabbitSettings
{
    public string? Url { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Exchange { get; set; }
    public string? Queue { get; set; }
    public string QueueName()
    {
        return $"{this.Exchange}_{this.Queue}";
    }
}
