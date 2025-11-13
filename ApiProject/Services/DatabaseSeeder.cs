using ApiProject.Data;
using ApiProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Services
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Check if data already exists
            if (await context.Categories.AnyAsync())
            {
                return; // Database has been seeded
            }

            // Seed Categories
            var categories = new List<Category>
            {
                new Category
                {
                    Name = "Shampoos",
                    Description = "Natural and professional shampoos for curly hair"
                },
                new Category
                {
                    Name = "Conditioners",
                    Description = "Deep conditioning treatments and leave-in conditioners"
                },
                new Category
                {
                    Name = "Styling Products",
                    Description = "Gels, creams, and styling products for curls"
                },
                new Category
                {
                    Name = "Hair Treatments",
                    Description = "Masks, oils, and intensive treatments"
                },
                new Category
                {
                    Name = "Tools & Accessories",
                    Description = "Brushes, diffusers, and hair accessories"
                }
            };

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();

            // Seed Products
            var products = new List<Product>
            {
                // Shampoos
                new Product
                {
                    Name = "Curl Defining Shampoo",
                    Description = "Sulfate-free shampoo that cleanses without stripping natural oils",
                    Price = 24.99m,
                    StockQuantity = 50,
                    ImageUrl = "https://example.com/shampoo1.jpg",
                    CategoryId = categories[0].Id
                },
                new Product
                {
                    Name = "Moisture Balance Shampoo",
                    Description = "Hydrating shampoo for dry and damaged curls",
                    Price = 29.99m,
                    StockQuantity = 45,
                    ImageUrl = "https://example.com/shampoo2.jpg",
                    CategoryId = categories[0].Id
                },

                // Conditioners
                new Product
                {
                    Name = "Deep Moisture Conditioner",
                    Description = "Intensive conditioner for maximum hydration",
                    Price = 27.99m,
                    StockQuantity = 60,
                    ImageUrl = "https://example.com/conditioner1.jpg",
                    CategoryId = categories[1].Id
                },
                new Product
                {
                    Name = "Leave-In Conditioner Spray",
                    Description = "Lightweight leave-in spray for everyday use",
                    Price = 19.99m,
                    StockQuantity = 75,
                    ImageUrl = "https://example.com/conditioner2.jpg",
                    CategoryId = categories[1].Id
                },

                // Styling Products
                new Product
                {
                    Name = "Curl Defining Gel",
                    Description = "Strong hold gel that defines curls without crunch",
                    Price = 22.99m,
                    StockQuantity = 40,
                    ImageUrl = "https://example.com/gel1.jpg",
                    CategoryId = categories[2].Id
                },
                new Product
                {
                    Name = "Curl Enhancing Cream",
                    Description = "Moisturizing cream that enhances natural curl pattern",
                    Price = 25.99m,
                    StockQuantity = 55,
                    ImageUrl = "https://example.com/cream1.jpg",
                    CategoryId = categories[2].Id
                },
                new Product
                {
                    Name = "Frizz Control Serum",
                    Description = "Lightweight serum that tames frizz and adds shine",
                    Price = 18.99m,
                    StockQuantity = 65,
                    ImageUrl = "https://example.com/serum1.jpg",
                    CategoryId = categories[2].Id
                },

                // Hair Treatments
                new Product
                {
                    Name = "Protein Hair Mask",
                    Description = "Strengthening mask for damaged hair",
                    Price = 32.99m,
                    StockQuantity = 30,
                    ImageUrl = "https://example.com/mask1.jpg",
                    CategoryId = categories[3].Id
                },
                new Product
                {
                    Name = "Argan Oil Treatment",
                    Description = "Pure argan oil for deep nourishment",
                    Price = 34.99m,
                    StockQuantity = 35,
                    ImageUrl = "https://example.com/oil1.jpg",
                    CategoryId = categories[3].Id
                },

                // Tools & Accessories
                new Product
                {
                    Name = "Wide Tooth Comb",
                    Description = "Detangling comb for curly hair",
                    Price = 12.99m,
                    StockQuantity = 100,
                    ImageUrl = "https://example.com/comb1.jpg",
                    CategoryId = categories[4].Id
                },
                new Product
                {
                    Name = "Microfiber Hair Towel",
                    Description = "Gentle towel that reduces frizz",
                    Price = 15.99m,
                    StockQuantity = 80,
                    ImageUrl = "https://example.com/towel1.jpg",
                    CategoryId = categories[4].Id
                },
                new Product
                {
                    Name = "Diffuser Attachment",
                    Description = "Universal diffuser for blow dryers",
                    Price = 28.99m,
                    StockQuantity = 45,
                    ImageUrl = "https://example.com/diffuser1.jpg",
                    CategoryId = categories[4].Id
                }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
    }
}
