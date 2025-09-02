using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        [Required]
        public int CustomerId { get; set; }
        
        public Customer Customer { get; set; } = null!;
        
        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";
        
        [Range(0.01, double.MaxValue)]
        public double TotalPrice { get; set; }
        
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}
