namespace Budget.RabbitMQ;

public interface IRabbitMqProducer
{
    public Task SendProductMessage<T>(T message);
}