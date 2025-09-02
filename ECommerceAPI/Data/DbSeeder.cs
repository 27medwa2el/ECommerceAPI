using ECommerceAPI.Models;

namespace ECommerceAPI.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ECommerceContext context)
        {
            // Ensuring the the DB is created
            await context.Database.EnsureCreatedAsync();

            // Checking if data already exists
            if (context.Products.Any())
            {
                return; // DB has been seeded
            }

            // Seeding the Products
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Laptop Dell XPS 13",
                    Description = "High-performance ultrabook with Intel Core i7 processor",
                    Price = 1299.99,
                    Stock = 25
                },
                new Product
                {
                    Name = "iPhone 14 Pro",
                    Description = "Latest Apple smartphone with advanced camera system",
                    Price = 999.99,
                    Stock = 50
                },
                new Product
                {
                    Name = "Samsung 4K Monitor",
                    Description = "27-inch 4K UHD monitor for professional use",
                    Price = 349.99,
                    Stock = 15
                },
                new Product
                {
                    Name = "Wireless Bluetooth Headphones",
                    Description = "Premium noise-cancelling wireless headphones",
                    Price = 199.99,
                    Stock = 40
                },
                new Product
                {
                    Name = "Gaming Mechanical Keyboard",
                    Description = "RGB backlit mechanical keyboard for gaming",
                    Price = 89.99,
                    Stock = 30
                },
                new Product
                {
                    Name = "Ergonomic Office Chair",
                    Description = "Comfortable office chair with lumbar support",
                    Price = 299.99,
                    Stock = 20
                },
                new Product
                {
                    Name = "Portable SSD 1TB",
                    Description = "High-speed external storage device",
                    Price = 149.99,
                    Stock = 35
                },
                new Product
                {
                    Name = "Webcam HD 1080p",
                    Description = "High-definition webcam for video conferencing",
                    Price = 79.99,
                    Stock = 60
                }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}
