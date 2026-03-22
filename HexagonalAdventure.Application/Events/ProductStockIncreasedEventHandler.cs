using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Domain.Events;

namespace HexagonalAdventure.Application.Events;

// Stok arttırma olayını dinleyen ve bu olay gerçekleştiğinde
// mesajlaşma altyapısı üzerinden ilgili bilgileri yayınlayan somut sınıfımız.
// ki burada mesajlaşma altyapısı IMessageBus arayüzü üzerinden enjekte edilir.
public class ProductStockIncreasedEventHandler(IMessageBus messageBus)
    : IDomainEventHandler<ProductStockIncreasedEvent>
{
    public async Task HandleAsync(ProductStockIncreasedEvent domainEvent)
    {
        await messageBus.PublishAsync(domainEvent, "stock-events-channel");
    }
}
