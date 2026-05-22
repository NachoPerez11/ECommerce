using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdWithItemsAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(Order order, CancellationToken ct = default);
    Task UpdateAsync(Order order, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}