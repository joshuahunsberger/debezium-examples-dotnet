using System.Text.Json;
using Confluent.Kafka;

namespace ShipmentService;

public class JsonDeserializer<T> : IDeserializer<T>
{
    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context) => JsonSerializer.Deserialize<T>(data);
}
