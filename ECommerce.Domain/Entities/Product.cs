namespace ECommerce.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Constructor vacío exigido por EF Core para la reconstrucción de datos
    private Product() { }

    // Constructor principal para tu lógica de negocio
    public Product(string name, string description, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del producto es obligatorio.");
        if (price < 0) 
            throw new ArgumentException("El precio no puede ser negativo.");
        if (stock < 0) 
            throw new ArgumentException("El stock no puede ser negativo.");

        Id = Guid.NewGuid(); // El ID se genera en el dominio, no en la BD
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0) throw new ArgumentException("Precio inválido.");
        Price = newPrice;
    }

    public void Restock(int quantity)
    {
        if (quantity < 0) throw new ArgumentException("Cantidad inválida.");
        Stock += quantity;
    }
}