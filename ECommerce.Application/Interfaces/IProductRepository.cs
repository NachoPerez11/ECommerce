using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task AddAsync(Product product);
    void Update(Product product);
    void Delete(Product product);
    Task<bool> SaveChangesAsync();
}