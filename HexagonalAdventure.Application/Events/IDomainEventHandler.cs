using HexagonalAdventure.Domain.Events;

namespace HexagonalAdventure.Application.Events;
/*
    IEventDispatcher bir domain olayının ilgili dinleyicilere dağıtacak sözleşme tanımı olarak düşünülebilir.
    IDomainEventHandler<TEvent> ise belirli bir domain olayını işlemekle ilgilenen dinleyiciler için tanımlanmış bir sözleşmesidir.
*/
public interface IEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent);
}

public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent);
}
