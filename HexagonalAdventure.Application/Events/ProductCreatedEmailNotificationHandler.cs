using HexagonalAdventure.Domain.Events;

namespace HexagonalAdventure.Application.Events;

// ProductCreatedEvent olayını dinleyen ve bu olay gerçekleştiğinde bir e-posta bildirimi göndermekle sorumlu olan somut sınıfımız
public class ProductCreatedEmailNotificationHandler
    : IDomainEventHandler<ProductCreatedEvent>
{
    public Task HandleAsync(ProductCreatedEvent domainEvent)
    {
        // Tabii ki bu çalışma ortamında gerçek bir e-posta gönderme işlemi yapmıyoruz.
        // Bu nedenle terminale bir mesaj yazdırarak simüle etsek yeterli.
        // Ama gerçekten email gönderimi yapmak istersek bunu da Outbound Port nesnesi olarak hazırlayıp buradan kullanmak gerekir.

        Console.WriteLine($"[Event] {domainEvent.Title} ({domainEvent.ProductCode}");
        return Task.CompletedTask;
    }
}
