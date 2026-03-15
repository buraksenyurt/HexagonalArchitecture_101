namespace HexagonalAdventure.Domain;

public class Product
{
    public Guid Id { get; private set; }
    public ProductCode Code { get; private set; }
    public Guid CategoryId { get; private set; }
    public string Title { get; private set; }
    public decimal ListPrice { get; private set; }
    public int StockQuantity { get; private set; } // Sonrasında Value Object olarak refactor edilebilir

    // EF Migration tool çalışırken parametresiz constructor'a ihtiyaç duyuyor, bu yüzden ekliyoruz.
    public Product()
    {

    }
    public Product(Guid id, ProductCode productCode, string title, decimal listPrice, Guid categoryId, int initialStock)
    {
        Id = id;
        Code = productCode ?? throw new ArgumentNullException(nameof(productCode), "Product code cannot be null");
        CategoryId = categoryId == Guid.Empty ? throw new ArgumentException("Category ID cannot be empty") : categoryId;
        Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Title cannot be empty") : title;
        ListPrice = listPrice > 0.0M ? listPrice : throw new ArgumentException("List price must be greater than 0.0");
        StockQuantity = initialStock >= 0 ? initialStock : throw new ArgumentException("Initial stock cannot be negative");
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity to increase must be greater than 0");

        StockQuantity += quantity;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity to decrease must be greater than 0");
        if (StockQuantity - quantity < 0) throw new InvalidOperationException("Insufficient stock to decrease by the specified quantity");

        StockQuantity -= quantity;
    }
}
