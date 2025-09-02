using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using ECommerceAPI.Models;
using ECommerceAPI.DTOs;
using ECommerceAPI.Repositories;
using ECommerceAPI.Validators;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly CreateCustomerValidator _validator;

        public CustomersController(ICustomerRepository customerRepository, CreateCustomerValidator validator)
        {
            _customerRepository = customerRepository;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            try
            {
                var customers = await _customerRepository.GetAllAsync();
                var customerDtos = customers.Select(c => new CustomerDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Phone = c.Phone
                });

                return Ok(customerDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving customers", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);
                
                if (customer == null)
                {
                    return NotFound(new { message = $"Customer with ID {id} not found" });
                }

                var customerDto = new CustomerDto
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    Phone = customer.Phone
                };

                return Ok(customerDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the customer", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(createCustomerDto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new { 
                        message = "Validation failed", 
                        errors = validationResult.Errors.Select(e => e.ErrorMessage) 
                    });
                }

                // Checkinng if email already exists
                if (await _customerRepository.EmailExistsAsync(createCustomerDto.Email))
                {
                    return BadRequest(new { message = "A customer with this email already exists" });
                }

                var customer = new Customer
                {
                    Name = createCustomerDto.Name,
                    Email = createCustomerDto.Email,
                    Phone = createCustomerDto.Phone
                };

                var createdCustomer = await _customerRepository.AddAsync(customer);

                var customerDto = new CustomerDto
                {
                    Id = createdCustomer.Id,
                    Name = createdCustomer.Name,
                    Email = createdCustomer.Email,
                    Phone = createdCustomer.Phone
                };

                return CreatedAtAction(nameof(GetCustomer), new { id = customerDto.Id }, customerDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the customer", details = ex.Message });
            }
        }
    }
}
