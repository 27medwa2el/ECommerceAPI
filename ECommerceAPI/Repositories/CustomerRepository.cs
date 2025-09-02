using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;
using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ECommerceContext context) : base(context)
        {
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(c => c.Email == email);
        }
    }
}
