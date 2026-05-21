namespace ECommerce.Api.DTOs;

public class CreateOrderDto
{
    public Guid UserId { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}