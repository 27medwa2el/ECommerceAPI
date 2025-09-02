using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;
using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ECommerceContext context) : base(context)
        {
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(o => o.Customer)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId)
        {
            return await _dbSet
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .ToListAsync();
        }
    }
}
