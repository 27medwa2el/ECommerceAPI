using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class OrderProduct
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        
        [Range(0.01, double.MaxValue)]
        public double Price { get; set; }
    }
}
