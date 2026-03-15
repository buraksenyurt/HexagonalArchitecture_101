namespace HexagonalAdventure.Domain;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }

    public Category(Guid id, string name)
    {
        Id = id;
        Name = string.IsNullOrEmpty(name) ? throw new ArgumentException("Category name cannot be empty") : name;
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
