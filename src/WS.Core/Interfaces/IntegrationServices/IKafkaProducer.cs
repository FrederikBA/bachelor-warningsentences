namespace WS.Core.Interfaces.IntegrationServices;

public interface IKafkaProducer : IDisposable
{
    Task ProduceAsync<T>(string topic, T value);
}