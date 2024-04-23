using System.Text.Json;
using Confluent.Kafka;
using WS.Core.Interfaces.Integration;
using Config = Shared.Integration.Configuration.Config;

namespace WS.Infrastructure.Producers;

public class SyncProducer : ISyncProducer
{
    private readonly IProducer<string, string> _producer;

    public SyncProducer()
    {
        var config = new ProducerConfig { BootstrapServers = Config.Kafka.BootstrapServers};
        _producer = new ProducerBuilder<string, string>(config).Build();
    }
    
    public async Task ProduceAsync<T>(string topic, T value)
    {
        var message = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(value)
        };
        await _producer.ProduceAsync(topic, message);
    }


    public void Dispose()
    {
        _producer.Dispose();
    }
}