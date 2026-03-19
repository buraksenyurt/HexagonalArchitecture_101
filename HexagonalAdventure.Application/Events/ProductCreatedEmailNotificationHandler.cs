using HexagonalAdventure.Application.Ports.Outbound;
using HexagonalAdventure.Domain.Events;

namespace HexagonalAdventure.Application.Events;

/*
    ProductCreatedEvent olayını dinleyen ve bu olay gerçekleştiğinde bir e-posta bildirimi göndermekten sorumlu olan somut sınıfımız.
    Email gönderme operasyonunu üstlenen servisimizi IEmailService üzerinden enjekte ediyoruz.
    HandleAsync metodu, ProductCreatedEvent tetiklendiğinde çağrılır ve e-posta gönderme işlemini gerçekleştirir.
    Bu ekleme testlerimizi de etkileyecektir, çünkü artık ürün oluşturulduğunda bir e-posta gönderilmesi bekleniyor. 
    Dolayısıyla bu sınıfın da test edilmesi gerekecek.
*/
public class ProductCreatedEmailNotificationHandler(IEmailService emailService)
    : IDomainEventHandler<ProductCreatedEvent>
{
    public async Task HandleAsync(ProductCreatedEvent domainEvent)
    {
        await emailService.SendEmailAsync(
            to: "salesops@hexagonaladventure.com",
            subject: $"New Product Created: {domainEvent.Title}",
            body: $"A new product has been created with the following details:\n\n" +
                  $"Product ID: {domainEvent.ProductId}\n" +
                  $"Product Code: {domainEvent.ProductCode}\n" +
                  $"Title: {domainEvent.Title}\n\n" +
                  $"Please review the new product and take any necessary actions."
         );
    }
}
