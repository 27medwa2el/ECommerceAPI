namespace ECommerceAPI.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int ProductCount { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
    }

    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
        public List<OrderProductDto> Products { get; set; } = new List<OrderProductDto>();
    }

    public class OrderProductDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateOrderStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }
}
