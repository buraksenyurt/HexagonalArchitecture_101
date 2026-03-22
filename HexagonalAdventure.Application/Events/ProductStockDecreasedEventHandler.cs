using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Domain.Events;

namespace HexagonalAdventure.Application.Events;

// Stok miktarı düşürme olayını dinleyen ve bu olay gerçekleştiğinde
// mesajlaşma altyapısı üzerinden ilgili bilgileri yayınlayan somut sınıfımız.
// ki burada mesajlaşma altyapısı IMessageBus arayüzü üzerinden enjekte edilir.
public class ProductStockDecreasedEventHandler(IMessageBus messageBus)
    : IDomainEventHandler<ProductStockDecreasedEvent>
{
    public async Task HandleAsync(ProductStockDecreasedEvent domainEvent)
    {
        await messageBus.PublishAsync(domainEvent, "stock-events-channel");
    }
}
