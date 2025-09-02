using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;
using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ECommerceContext context) : base(context)
        {
        }

        public async Task<bool> HasEnoughStockAsync(int productId, int quantity)
        {
            var product = await _dbSet.FindAsync(productId);
            return product != null && product.Stock >= quantity;
        }

        public async Task UpdateStockAsync(int productId, int quantity)
        {
            var product = await _dbSet.FindAsync(productId);
            if (product != null)
            {
                product.Stock -= quantity;
                await _context.SaveChangesAsync();
            }
        }
    }
}
