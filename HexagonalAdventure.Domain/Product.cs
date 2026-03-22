using HexagonalAdventure.Domain.Events;

namespace HexagonalAdventure.Domain;

public class Product
{
    public Guid Id { get; private set; }
    public ProductCode Code { get; private set; }
    public Guid CategoryId { get; private set; }
    public string Title { get; private set; }
    public decimal ListPrice { get; private set; }
    public int StockQuantity { get; private set; } // Sonrasında Value Object olarak refactor edilebilir

    /*
        Bu entity ile ilişkili domain event'leri bir liste olarak tutuacağız. 
        Dışarıdan erişmek isteyenler için sadece okunabilir bir liste sunacağız.
    */
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

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

        // Bu satıra geldiğimizde yukarıdaki temek domain kurallarının başarılı bir şekilde geçildiğini
        // ve bir ürün oluşturulduğunu düşünebiliriz. Bunu sistemde bir event olarak ele almak için de domain event olarak ekliyoruz.
        AddDomainEvent(new ProductCreatedEvent(Id, Code.Value, Title));
        /*
            Not: Akla şu soru gelebilir. Neden olayı sisteme fırlatmıyor ve bir listeye ekliyoruz.
            Domain katmanından yukarı çıkalım ve Product nesnesinin başarılı ile oluşturulmasının sisteme bir ürün kaydedildiği
            anlamına gelip gelmediğini düşünelim. Büyük ihtimalle nesne bir veritabanına veya başka bir fiziki ortama yazılacaktır.
            Yani sisteme eklenmiş bir üründen bahsetmek için arada geçilmesi gereken bir takım iş kuralları ve katmanlar olabilir.
            Buna bağlı olarak bir ürün başarılı şekilde sisteme girmeden bildirimlerde bulunmak hatalı bir davranış olur.
            Düşünsenize; ürün eklenemedi ama patrona bayilere stoğa yeni ürün eklendiğine dair bilgi gitti :D
            Bu yüzden olayları entity nesnesi içerisine biriktiriyoruz. Sonrasında işleyeceğiz.         
        */
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity to increase must be greater than 0");

        StockQuantity += quantity;
        AddDomainEvent(new ProductStockIncreasedEvent(Id, quantity));
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity to decrease must be greater than 0");
        if (StockQuantity - quantity < 0) throw new InvalidOperationException("Insufficient stock to decrease by the specified quantity");

        StockQuantity -= quantity;
        AddDomainEvent(new ProductStockDecreasedEvent(Id, quantity));
    }

    // Bu entity'ye yeni bir domain event eklemek için kullanılan yardımcı metodumuz
    private void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    // Dışarıdan erişilebilen ve domain event'leri temizlemek için kullanılan yardımcı metodumuz
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
