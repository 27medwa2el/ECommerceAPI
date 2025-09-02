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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly CreateOrderValidator _createValidator;
        private readonly UpdateOrderStatusValidator _updateValidator;

        public OrdersController(
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            CreateOrderValidator createValidator,
            UpdateOrderStatusValidator updateValidator)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto createOrderDto)
        {
            try
            {
                var validationResult = await _createValidator.ValidateAsync(createOrderDto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new { 
                        message = "Validation failed", 
                        errors = validationResult.Errors.Select(e => e.ErrorMessage) 
                    });
                }

                // Verifing that customerr exists
                var customer = await _customerRepository.GetByIdAsync(createOrderDto.CustomerId);
                if (customer == null)
                {
                    return BadRequest(new { message = "Customer not found" });
                }

                // Verifing all products exist and have enough stock
                var orderProducts = new List<OrderProduct>();
                double totalPrice = 0;

                foreach (var productDto in createOrderDto.Products)
                {
                    var product = await _productRepository.GetByIdAsync(productDto.ProductId);
                    if (product == null)
                    {
                        return BadRequest(new { message = $"Product with ID {productDto.ProductId} not found" });
                    }

                    if (!await _productRepository.HasEnoughStockAsync(productDto.ProductId, productDto.Quantity))
                    {
                        return BadRequest(new { message = $"Insufficient stock for product {product.Name}" });
                    }

                    var orderProduct = new OrderProduct
                    {
                        ProductId = productDto.ProductId,
                        Quantity = productDto.Quantity,
                        Price = product.Price
                    };

                    orderProducts.Add(orderProduct);
                    totalPrice += product.Price * productDto.Quantity;
                }

                // Create the order
                var order = new Order
                {
                    CustomerId = createOrderDto.CustomerId,
                    OrderDate = DateTime.UtcNow,
                    Status = "Pending",
                    TotalPrice = totalPrice,
                    OrderProducts = orderProducts
                };

                var createdOrder = await _orderRepository.AddAsync(order);

                var orderDto = new OrderDto
                {
                    Id = createdOrder.Id,
                    CustomerName = customer.Name,
                    Status = createdOrder.Status,
                    ProductCount = createdOrder.OrderProducts.Count,
                    OrderDate = createdOrder.OrderDate,
                    TotalPrice = createdOrder.TotalPrice
                };

                return CreatedAtAction(nameof(GetOrder), new { id = orderDto.Id }, orderDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the order", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            try
            {
                var order = await _orderRepository.GetOrderWithDetailsAsync(id);
                
                if (order == null)
                {
                    return NotFound(new { message = $"Order with ID {id} not found" });
                }

                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    CustomerName = order.Customer.Name,
                    Status = order.Status,
                    ProductCount = order.OrderProducts.Count,
                    OrderDate = order.OrderDate,
                    TotalPrice = order.TotalPrice
                };

                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the order", details = ex.Message });
            }
        }

        [HttpPost("UpdateOrderStatus/{id}")]
        public async Task<ActionResult> UpdateOrderStatus(int id, UpdateOrderStatusDto updateStatusDto)
        {
            try
            {
                var validationResult = await _updateValidator.ValidateAsync(updateStatusDto);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new { 
                        message = "Validation failed", 
                        errors = validationResult.Errors.Select(e => e.ErrorMessage) 
                    });
                }

                var order = await _orderRepository.GetOrderWithDetailsAsync(id);
                if (order == null)
                {
                    return NotFound(new { message = $"Order with ID {id} not found" });
                }

                order.Status = updateStatusDto.Status;

                // If order is being marked as delivered , then update product stock
                if (updateStatusDto.Status == "Delivered" && order.Status != "Delivered")
                {
                    foreach (var orderProduct in order.OrderProducts)
                    {
                        await _productRepository.UpdateStockAsync(orderProduct.ProductId, orderProduct.Quantity);
                    }
                }

                await _orderRepository.UpdateAsync(order);

                return Ok(new { message = "Order status updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the order status", details = ex.Message });
            }
        }
    }
}
