using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<bool> HasEnoughStockAsync(int productId, int quantity);
        Task UpdateStockAsync(int productId, int quantity);
    }
}
