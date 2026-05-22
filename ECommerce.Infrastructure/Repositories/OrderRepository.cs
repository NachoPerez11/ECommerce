using Microsoft.EntityFrameworkCore;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;

namespace ECommerce.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _ctx;
    public OrderRepository(ApplicationDbContext ctx) => _ctx = ctx;

    
    public async Task<Order?> GetByIdWithItemsAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Orders
        .Include(o => o.Items)  // JOIN con OrderItems [cite: 54]
        .FirstOrDefaultAsync(o => o.Id == id, ct);

    public async Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _ctx.Orders
               .AsNoTracking()
               .Where(o => o.UserId == userId)
               .OrderByDescending(o => o.CreatedAt)
               .ToListAsync(ct);

    public async Task AddAsync(Order order, CancellationToken ct = default)
    {
        await _ctx.Orders.AddAsync(order, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Order order, CancellationToken ct = default)
    {
        _ctx.Orders.Update(order);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var order = await _ctx.Orders.FindAsync(new object[] { id }, ct);
        if (order != null)
        {
            _ctx.Orders.Remove(order);
            await _ctx.SaveChangesAsync(ct);
        }
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _ctx.SaveChangesAsync(ct);
    }
}