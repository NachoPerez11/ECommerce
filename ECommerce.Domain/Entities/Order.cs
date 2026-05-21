namespace ECommerce.Domain.Entities;

public enum OrderStatus { Pending, Confirmed, Shipped, Delivered, Cancelled }

public class Order
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal Total { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    // para EF Core
    private Order() { }

    public Order(Guid userId)
    {
        Id        = Guid.NewGuid();
        UserId    = userId;
        CreatedAt = DateTime.UtcNow;
        Status    = OrderStatus.Pending;
        Total     = 0;
    }

    public void AddItem(Product product, int quantity)
    {
        if (product.Stock < quantity)
            throw new InvalidOperationException("Insufficient stock.");
        var item = new OrderItem(Id, product.Id, product.Price, quantity);
        _items.Add(item);
        Total += item.Subtotal;
    }
}