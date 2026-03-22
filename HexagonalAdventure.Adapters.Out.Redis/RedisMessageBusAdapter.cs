using HexagonalAdventure.Application.Ports.Outbound;
using StackExchange.Redis;
using System.Text.Json;

namespace HexagonalAdventure.Adapters.Out.Redis;

/*
    Bir başka Outbound adapter nesnemiz.
    Bu sefer Redis'e mesaj gönderiyoruz.
*/
public class RedisMessageBusAdapter(IConnectionMultiplexer redisConnection)
    : IMessageBus
{
    public async Task PublishAsync<TMessage>(TMessage message, string topicName)
    {
        var subscriber = redisConnection.GetSubscriber();
        var payload = JsonSerializer.Serialize(message);
        await subscriber.PublishAsync(RedisChannel.Literal(topicName), payload);
    }
}
