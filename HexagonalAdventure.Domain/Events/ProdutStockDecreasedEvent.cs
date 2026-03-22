namespace HexagonalAdventure.Domain.Events;

public record ProductStockDecreasedEvent(Guid ProductId, int Quantity) : IDomainEvent;
