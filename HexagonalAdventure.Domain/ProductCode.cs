namespace HexagonalAdventure.Domain;

/*
    Ürün kodu bir Value Object olarak tasarlanmıştır.
    DDD yaklaşımına göre Immutable (değiştirilemez) bir yapıya sahip olmalıdır
    ve değişiklik yapılması gerektiğinde yeni bir instance oluşturulmalıdır.
    Entity'ler genellikle benzersiz bir kimlik (ID) ile tanımlanırken, 
    Value Object'ler sadece değerleriyle tanımlanır ve kimlik taşımazlar.
    Entity'ler ID ile ifade edildiklerinden özellikleri değişse bile kimliği aynı kaldığı süre hep aynı nesne olarak düşünülürler.
    Value Object'ler ise sadece değerleriyle tanımlandıkları için iki Value Object aynı değerlere sahipse birbirleriyle eşit kabul edilirler.
 */
public record ProductCode
{
    public string Value { get; }

    public ProductCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Product code cannot be null or empty.", nameof(value));

        if (value.Length < 5 || value.Contains(' '))
            throw new ArgumentException("Product code must be at least 5 characters long and cannot contain spaces.", nameof(value));

        Value = value.ToUpperInvariant();
    }
}
