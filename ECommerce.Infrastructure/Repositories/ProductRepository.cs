using Microsoft.EntityFrameworkCore;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;

namespace ECommerce.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Products.FindAsync(id, ct);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Products.ToListAsync();
    }

    public async Task AddAsync(Product product, CancellationToken ct = default)
    {
        await _context.Products.AddAsync(product, ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var product = await _context.Products.FindAsync(id, ct);
        if (product != null)
        {
            _context.Products.Remove(product);
        }
    }

    public async Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        _context.Products.Update(product);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct) > 0;
    }
}