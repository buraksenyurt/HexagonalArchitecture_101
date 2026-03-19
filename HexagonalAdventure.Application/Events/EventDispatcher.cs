using HexagonalAdventure.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace HexagonalAdventure.Application.Events;

/*
    Dispatcher ya da kaynaklarda postacı olarak geçen kıymetli nesnemiz :D
    Bu sınıf .Net'in IServiceProvider konteynırından yararlanarak olayları dinleyenleri 
    dinamik olarak bulup haberdar etmekten sorumludur.
*/

public class EventDispatcher(IServiceProvider serviceProvider)
    : IEventDispatcher
{
    public async Task DispatchAsync(IDomainEvent domainEvent)
    {
        // Arayüz üzerinden gelen asıl olay türünü buluruz. Örneğin ProductCreatedEvent gibi.
        var eventType = domainEvent.GetType();
        // Buna göre bir IDomainEventHandler<T> türü dinamik olarak oluşturulur. Örneğin IDomainEventHandler<ProductCreatedEvent>.
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);
        // Dependency Injection Container'a gidilir ve IDomainEventHandler arayüzünü uygulayan tüm sınıflar bulunur.
        // Örneğin ProductCreatedEmailNotificationHandler gibi.
        var handlers = serviceProvider.GetServices(handlerType); // Microsoft.Extensions.DependencyInjection paketini gerektirir

        // Olayla ilgili dinleyiciler bulunduğuna göre her birinin HandleAsync metodunu yakalanı ve çağrılır.
        // Bu sayede olay meydana geldiğinde, bu olaya abone olan tüm sınıflar haberdar edilir.
        foreach (var handler in handlers)
        {
            var method = handlerType.GetMethod("HandleAsync");
            if (method != null)
            {
                await (Task)method.Invoke(handler, [domainEvent])!;
            }
        }
    }
}
