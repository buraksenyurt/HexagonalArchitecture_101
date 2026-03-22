namespace HexagonalAdventure.Domain.Events;

public record ProductStockIncreasedEvent(Guid ProductId, int Quantity) : IDomainEvent;
