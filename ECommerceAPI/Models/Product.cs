using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        [Range(0.01, double.MaxValue)]
        public double Price { get; set; }
        
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
        
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}
