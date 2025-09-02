using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetOrderWithDetailsAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId);
    }
}
