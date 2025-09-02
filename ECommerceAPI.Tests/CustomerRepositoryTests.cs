using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories;

namespace ECommerceAPI.Tests
{
    public class CustomerRepositoryTests : IDisposable
    {
        private readonly ECommerceContext _context;
        private readonly CustomerRepository _repository;

        public CustomerRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ECommerceContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ECommerceContext(options);
            _repository = new CustomerRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldCreateCustomer_WhenValidDataProvided()
        {
            // Arrange
            var customer = new Customer
            {
                Name = "John Doe",
                Email = "john@example.com",
                Phone = "123456789"
            };

            // Act
            var result = await _repository.AddAsync(customer);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal("John Doe", result.Name);
            Assert.Equal("john@example.com", result.Email);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnCustomer_WhenEmailExists()
        {
            // Arrange
            var customer = new Customer
            {
                Name = "Jane Smith",
                Email = "jane@example.com",
                Phone = "987654321"
            };
            await _repository.AddAsync(customer);

            // Act
            var result = await _repository.GetByEmailAsync("jane@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Jane Smith", result.Name);
            Assert.Equal("jane@example.com", result.Email);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnNull_WhenEmailDoesNotExist()
        {
            // Act
            var result = await _repository.GetByEmailAsync("nonexistent@example.com");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task EmailExistsAsync_ShouldReturnTrue_WhenEmailExists()
        {
            // Arrange
            var customer = new Customer
            {
                Name = "Test User",
                Email = "test@example.com",
                Phone = "555123456"
            };
            await _repository.AddAsync(customer);

            // Act
            var result = await _repository.EmailExistsAsync("test@example.com");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task EmailExistsAsync_ShouldReturnFalse_WhenEmailDoesNotExist()
        {
            // Act
            var result = await _repository.EmailExistsAsync("nonexistent@example.com");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Name = "Customer 1", Email = "customer1@example.com", Phone = "111111111" },
                new Customer { Name = "Customer 2", Email = "customer2@example.com", Phone = "222222222" },
                new Customer { Name = "Customer 3", Email = "customer3@example.com", Phone = "333333333" }
            };

            foreach (var customer in customers)
            {
                await _repository.AddAsync(customer);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(3, result.Count());
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
