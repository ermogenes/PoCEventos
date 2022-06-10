using Confluent.Kafka;
using StackExchange.Redis;

namespace Web.kafka;

public class DeliveryHandler
{
    private ILogger<DeliveryHandler> _logger;
    private IDatabase _redis;

    public DeliveryHandler(
        ILogger<DeliveryHandler> logger,
        IConnectionMultiplexer connmux
    )
    {
        _logger = logger;
        _redis = connmux.GetDatabase();
    }

    public async void DeliveryReportHandler(
        DeliveryReport<long, string> deliveryReport
    )
    {
        var key = deliveryReport.Message.Key;
        var value = deliveryReport.Message.Value;

        switch (deliveryReport.Status)
        {
            case PersistenceStatus.NotPersisted:
                var txNP = _redis.CreateTransaction();
                long npCount = await txNP.StringIncrementAsync("np");
                await txNP.StringSetAsync($"np:{key}", value);
                await txNP.ExecuteAsync();
                _logger.LogError($"NotPersisted #{npCount}: {key} = {value}");
                break;
            case PersistenceStatus.PossiblyPersisted:
                var txPP = _redis.CreateTransaction();
                long ppCount = await txPP.StringIncrementAsync("pp");
                await txPP.StringSetAsync($"pp:{key}", value);
                await txPP.ExecuteAsync();
                _logger.LogWarning($"PossiblyPersisted #{ppCount}: {key} = {value}");
                break;
            case PersistenceStatus.Persisted:
                await _redis.StringIncrementAsync("p");
                break;
        }
    }
}
