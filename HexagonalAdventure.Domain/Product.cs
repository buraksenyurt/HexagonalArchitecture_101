namespace HexagonalAdventure.Domain;

public class Product
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public decimal ListPrice { get; private set; }
    public string Category { get; private set; } // Category ayrı bir entity olabilir, şimdilik string olarak bıraktım
    public int StockQuantity { get; private set; } // Sonrasında Value Object olarak refactor edilebilir

    // EF Migration tool çalışırken parametresiz constructor'a ihtiyaç duyuyor, bu yüzden ekliyoruz.
    public Product()
    {
        
    }
    public Product(Guid id, string title, decimal listPrice, string category, int initialStock)
    {
        Id = id;
        Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Title cannot be empty") : title;
        ListPrice = listPrice > 0.0M ? listPrice : throw new ArgumentException("List price must be greater than 0.0");
        Category = string.IsNullOrWhiteSpace(category) ? throw new ArgumentException("Category cannot be empty") : category;
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
