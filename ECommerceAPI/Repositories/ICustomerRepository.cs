using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
    }
}
