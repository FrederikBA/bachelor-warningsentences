using Confluent.Kafka;
using System.Text.Json;
using WS.Core.Interfaces.Integration;

namespace WS.Test.Drivers;

public class TestProducer : ISyncProducer
{
    private readonly IProducer<string, string> _producer;
    private const string BootstrapServers = "localhost:9092";

    public TestProducer()
    {
        var config = new ProducerConfig { BootstrapServers = BootstrapServers };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task ProduceAsync<T>(string topic, T value)
    {
        var message = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(value)
        };
        await _producer.ProduceAsync("test-topic-ws", message);
    }


    public void Dispose()
    {
        _producer.Dispose();
    }
}