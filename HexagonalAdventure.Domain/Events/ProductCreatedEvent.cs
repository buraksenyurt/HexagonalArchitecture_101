namespace HexagonalAdventure.Domain.Events;

/*
    Olayları, yaşanmış bitmiş eylemler olarak düşünelim.
    Bu nedenle geçmiş zaman kipinden isimlendirmek oldukça mantıklı olur.
    Ayrıca değerleri değişmeyecek şekilde tanımlanır (immutable). Bu nedenle record türünü kullanabiliriz.
*/
public record ProductCreatedEvent(Guid ProductId, string ProductCode, string Title) : IDomainEvent;
