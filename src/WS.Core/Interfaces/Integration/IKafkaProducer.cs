namespace WS.Core.Interfaces.Integration;

public interface IKafkaProducer : IDisposable
{
    Task ProduceAsync<T>(string topic, T value);
}