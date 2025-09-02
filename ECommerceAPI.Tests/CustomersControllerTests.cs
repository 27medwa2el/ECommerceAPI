using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Controllers;
using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories;
using ECommerceAPI.Validators;

namespace ECommerceAPI.Tests
{
    public class CustomersControllerTests : IDisposable
    {
        private readonly ECommerceContext _context;
        private readonly CustomerRepository _repository;
        private readonly CustomersController _controller;
        private readonly CreateCustomerValidator _validator;

        public CustomersControllerTests()
        {
            var options = new DbContextOptionsBuilder<ECommerceContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ECommerceContext(options);
            _repository = new CustomerRepository(_context);
            _validator = new CreateCustomerValidator();
            _controller = new CustomersController(_repository, _validator);
        }

        [Fact]
        public async Task CreateCustomer_ShouldReturnCreatedResult_WhenValidDataProvided()
        {
            // Arrange
            var createCustomerDto = new CreateCustomerDto
            {
                Name = "John Doe",
                Email = "john@example.com",
                Phone = "123456789"
            };

            // Act
            var result = await _controller.CreateCustomer(createCustomerDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var customerDto = Assert.IsType<CustomerDto>(createdResult.Value);
            Assert.Equal("John Doe", customerDto.Name);
            Assert.Equal("john@example.com", customerDto.Email);
        }

        [Fact]
        public async Task CreateCustomer_ShouldReturnBadRequest_WhenEmailAlreadyExists()
        {
            // Arrange
            var existingCustomer = new Customer
            {
                Name = "Existing User",
                Email = "duplicate@example.com",
                Phone = "123456789"
            };
            await _repository.AddAsync(existingCustomer);

            var createCustomerDto = new CreateCustomerDto
            {
                Name = "New User",
                Email = "duplicate@example.com",
                Phone = "987654321"
            };

            // Act
            var result = await _controller.CreateCustomer(createCustomerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task CreateCustomer_ShouldReturnBadRequest_WhenRequiredFieldsMissing()
        {
            // Arrange
            var createCustomerDto = new CreateCustomerDto
            {
                Name = "",
                Email = "",
                Phone = "123456789"
            };

            // Act
            var result = await _controller.CreateCustomer(createCustomerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task GetCustomer_ShouldReturnOkResult_WhenCustomerExists()
        {
            // Arrange
            var customer = new Customer
            {
                Name = "Test User",
                Email = "test@example.com",
                Phone = "123456789"
            };
            var createdCustomer = await _repository.AddAsync(customer);

            // Act
            var result = await _controller.GetCustomer(createdCustomer.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var customerDto = Assert.IsType<CustomerDto>(okResult.Value);
            Assert.Equal("Test User", customerDto.Name);
            Assert.Equal("test@example.com", customerDto.Email);
        }

        [Fact]
        public async Task GetCustomer_ShouldReturnNotFound_WhenCustomerDoesNotExist()
        {
            // Act
            var result = await _controller.GetCustomer(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.NotNull(notFoundResult.Value);
        }

        [Fact]
        public async Task GetCustomers_ShouldReturnAllCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Name = "Customer 1", Email = "customer1@example.com", Phone = "111111111" },
                new Customer { Name = "Customer 2", Email = "customer2@example.com", Phone = "222222222" }
            };

            foreach (var customer in customers)
            {
                await _repository.AddAsync(customer);
            }

            // Act
            var result = await _controller.GetCustomers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var customerDtos = Assert.IsAssignableFrom<IEnumerable<CustomerDto>>(okResult.Value);
            Assert.Equal(2, customerDtos.Count());
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
