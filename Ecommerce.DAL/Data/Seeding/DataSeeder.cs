using Ecommerce.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Data.Seeding
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(AppDbContext context, UserManager<AppUser> userManager)
        {
            await context.Database.MigrateAsync();

            // Seed Categories
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Smartphones",
                        Description = "Mobile phones and smartphones with latest technology",
                        ImageURL = "/images/categories/smartphones.jpg"
                    },
                    new Category
                    {
                        Name = "Laptops",
                        Description = "Computers and laptops for work and gaming",
                        ImageURL = "/images/categories/laptops.jpg"
                    },
                    new Category
                    {
                        Name = "Tablets",
                        Description = "Portable tablets and iPad devices",
                        ImageURL = "/images/categories/tablets.jpg"
                    },
                    new Category
                    {
                        Name = "Audio",
                        Description = "Headphones, speakers, and audio equipment",
                        ImageURL = "/images/categories/audio.jpg"
                    },
                    new Category
                    {
                        Name = "Wearables",
                        Description = "Smartwatches and fitness trackers",
                        ImageURL = "/images/categories/wearables.jpg"
                    },
                    new Category
                    {
                        Name = "Accessories",
                        Description = "Phone cases, chargers, cables, and other accessories",
                        ImageURL = "/images/categories/accessories.jpg"
                    }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // Seed Products
            if (!context.Products.Any())
            {
                var categories = await context.Categories.ToListAsync();
                var smartphonesCategory = categories.FirstOrDefault(c => c.Name == "Smartphones");
                var laptopsCategory = categories.FirstOrDefault(c => c.Name == "Laptops");
                var tabletsCategory = categories.FirstOrDefault(c => c.Name == "Tablets");
                var audioCategory = categories.FirstOrDefault(c => c.Name == "Audio");
                var wearablesCategory = categories.FirstOrDefault(c => c.Name == "Wearables");
                var accessoriesCategory = categories.FirstOrDefault(c => c.Name == "Accessories");

                var products = new List<Product>
                {
                    // Smartphones
                    new Product
                    {
                        Name = "iPhone 15 Pro Max",
                        Description = "Apple's latest flagship smartphone with A17 Pro chip and advanced camera system",
                        Price = 1199.99m,
                        Stock = 50,
                        ImageURL = "/images/products/iphone15promax.jpg",
                        CategoryId = smartphonesCategory.Id
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy S24 Ultra",
                        Description = "Premium Android flagship with S Pen and incredible display",
                        Price = 1299.99m,
                        Stock = 45,
                        ImageURL = "/images/products/galaxys24ultra.jpg",
                        CategoryId = smartphonesCategory.Id
                    },
                    new Product
                    {
                        Name = "Google Pixel 8 Pro",
                        Description = "Google's flagship with advanced AI processing and exceptional camera",
                        Price = 999.99m,
                        Stock = 40,
                        ImageURL = "/images/products/pixel8pro.jpg",
                        CategoryId = smartphonesCategory.Id
                    },
                    new Product
                    {
                        Name = "OnePlus 12",
                        Description = "Fast performance with 5G connectivity and vibrant AMOLED display",
                        Price = 799.99m,
                        Stock = 60,
                        ImageURL = "/images/products/oneplus12.jpg",
                        CategoryId = smartphonesCategory.Id
                    },
                    new Product
                    {
                        Name = "iPhone 15",
                        Description = "Apple's standard flagship with A16 Bionic chip",
                        Price = 799.99m,
                        Stock = 70,
                        ImageURL = "/images/products/iphone15.jpg",
                        CategoryId = smartphonesCategory.Id
                    },

                    // Laptops
                    new Product
                    {
                        Name = "MacBook Pro 16\" M3 Max",
                        Description = "Powerful laptop for professionals with impressive performance",
                        Price = 3499.99m,
                        Stock = 25,
                        ImageURL = "/images/products/macbookpro16.jpg",
                        CategoryId = laptopsCategory.Id
                    },
                    new Product
                    {
                        Name = "Dell XPS 15",
                        Description = "Premium Windows laptop with stunning display and powerful performance",
                        Price = 2499.99m,
                        Stock = 30,
                        ImageURL = "/images/products/xps15.jpg",
                        CategoryId = laptopsCategory.Id
                    },
                    new Product
                    {
                        Name = "ASUS ROG Zephyrus G16",
                        Description = "High-end gaming laptop with RTX 4090 and 240Hz display",
                        Price = 3199.99m,
                        Stock = 20,
                        ImageURL = "/images/products/rogzephyrus.jpg",
                        CategoryId = laptopsCategory.Id
                    },
                    new Product
                    {
                        Name = "Lenovo ThinkPad X1 Carbon",
                        Description = "Business laptop with excellent build quality and battery life",
                        Price = 1799.99m,
                        Stock = 35,
                        ImageURL = "/images/products/thinkpadx1.jpg",
                        CategoryId = laptopsCategory.Id
                    },
                    new Product
                    {
                        Name = "HP Spectre x360 16",
                        Description = "Convertible laptop with OLED display and premium design",
                        Price = 2199.99m,
                        Stock = 28,
                        ImageURL = "/images/products/spectrex360.jpg",
                        CategoryId = laptopsCategory.Id
                    },

                    // Tablets
                    new Product
                    {
                        Name = "iPad Pro 12.9\" (M2)",
                        Description = "Apple's most powerful tablet with stunning display and Apple Pencil support",
                        Price = 1099.99m,
                        Stock = 30,
                        ImageURL = "/images/products/ipadpro129.jpg",
                        CategoryId = tabletsCategory.Id
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy Tab S9 Ultra",
                        Description = "Premium Android tablet with large 14.6\" display",
                        Price = 1199.99m,
                        Stock = 25,
                        ImageURL = "/images/products/tabs9ultra.jpg",
                        CategoryId = tabletsCategory.Id
                    },
                    new Product
                    {
                        Name = "iPad Air 11\"",
                        Description = "Mid-range iPad with M1 chip and vibrant display",
                        Price = 799.99m,
                        Stock = 40,
                        ImageURL = "/images/products/ipadair11.jpg",
                        CategoryId = tabletsCategory.Id
                    },
                    new Product
                    {
                        Name = "Microsoft Surface Pro 10",
                        Description = "2-in-1 tablet and laptop with Windows 11",
                        Price = 1299.99m,
                        Stock = 20,
                        ImageURL = "/images/products/surfacepro10.jpg",
                        CategoryId = tabletsCategory.Id
                    },

                    // Audio
                    new Product
                    {
                        Name = "Sony WH-1000XM5 Headphones",
                        Description = "Premium noise-cancelling headphones with exceptional audio quality",
                        Price = 399.99m,
                        Stock = 80,
                        ImageURL = "/images/products/wh1000xm5.jpg",
                        CategoryId = audioCategory.Id
                    },
                    new Product
                    {
                        Name = "Apple AirPods Pro (2nd Gen)",
                        Description = "Wireless earbuds with active noise cancellation and spatial audio",
                        Price = 249.99m,
                        Stock = 100,
                        ImageURL = "/images/products/airpodspro2.jpg",
                        CategoryId = audioCategory.Id
                    },
                    new Product
                    {
                        Name = "Bose QuietComfort 45",
                        Description = "Comfort-focused noise-cancelling headphones",
                        Price = 379.99m,
                        Stock = 60,
                        ImageURL = "/images/products/qc45.jpg",
                        CategoryId = audioCategory.Id
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy Buds2 Pro",
                        Description = "Premium wireless earbuds with IPX7 water resistance",
                        Price = 229.99m,
                        Stock = 90,
                        ImageURL = "/images/products/buds2pro.jpg",
                        CategoryId = audioCategory.Id
                    },
                    new Product
                    {
                        Name = "JBL Flip 6 Speaker",
                        Description = "Portable waterproof Bluetooth speaker with great sound",
                        Price = 129.99m,
                        Stock = 120,
                        ImageURL = "/images/products/flip6.jpg",
                        CategoryId = audioCategory.Id
                    },

                    // Wearables
                    new Product
                    {
                        Name = "Apple Watch Series 9",
                        Description = "Advanced smartwatch with health monitoring and fitness tracking",
                        Price = 399.99m,
                        Stock = 70,
                        ImageURL = "/images/products/applewatch9.jpg",
                        CategoryId = wearablesCategory.Id
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy Watch 6 Classic",
                        Description = "Premium Android smartwatch with rotating bezel",
                        Price = 399.99m,
                        Stock = 50,
                        ImageURL = "/images/products/galaxywatch6.jpg",
                        CategoryId = wearablesCategory.Id
                    },
                    new Product
                    {
                        Name = "Garmin Epix (Gen 2)",
                        Description = "Sports-focused smartwatch with offline maps",
                        Price = 649.99m,
                        Stock = 35,
                        ImageURL = "/images/products/epix.jpg",
                        CategoryId = wearablesCategory.Id
                    },
                    new Product
                    {
                        Name = "Fitbit Sense 2",
                        Description = "Fitness tracker with health monitoring features",
                        Price = 299.99m,
                        Stock = 80,
                        ImageURL = "/images/products/sense2.jpg",
                        CategoryId = wearablesCategory.Id
                    },

                    // Accessories
                    new Product
                    {
                        Name = "Apple MagSafe Charger",
                        Description = "Magnetic wireless charger for iPhones",
                        Price = 39.99m,
                        Stock = 200,
                        ImageURL = "/images/products/magsafecharger.jpg",
                        CategoryId = accessoriesCategory.Id
                    },
                    new Product
                    {
                        Name = "Anker PowerBank 30000mAh",
                        Description = "High-capacity portable power bank with fast charging",
                        Price = 69.99m,
                        Stock = 150,
                        ImageURL = "/images/products/ankerpower.jpg",
                        CategoryId = accessoriesCategory.Id
                    },
                    new Product
                    {
                        Name = "Spigen iPhone 15 Pro Case",
                        Description = "Protective phone case with premium materials",
                        Price = 24.99m,
                        Stock = 300,
                        ImageURL = "/images/products/spigencase.jpg",
                        CategoryId = accessoriesCategory.Id
                    },
                    new Product
                    {
                        Name = "Belkin USB-C Cable 3m",
                        Description = "Durable high-speed USB-C charging cable",
                        Price = 19.99m,
                        Stock = 250,
                        ImageURL = "/images/products/belfincable.jpg",
                        CategoryId = accessoriesCategory.Id
                    },
                    new Product
                    {
                        Name = "Apple AirTag",
                        Description = "Small Bluetooth tracker for finding your items",
                        Price = 29.99m,
                        Stock = 180,
                        ImageURL = "/images/products/airtag.jpg",
                        CategoryId = accessoriesCategory.Id
                    },
                    new Product
                    {
                        Name = "Native Union Phone Stand",
                        Description = "Premium aluminum phone stand for desk",
                        Price = 34.99m,
                        Stock = 120,
                        ImageURL = "/images/products/phonestand.jpg",
                        CategoryId = accessoriesCategory.Id
                    }
                };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }

            // Seed Default Users
            if (!context.Users.Any())
            {
                var adminUser = new AppUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }

                var regularUser = new AppUser
                {
                    UserName = "user@example.com",
                    Email = "user@example.com",
                    FirstName = "John",
                    LastName = "Doe",
                    EmailConfirmed = true
                };

                result = await userManager.CreateAsync(regularUser, "User@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(regularUser, "User");
                }
            }
        }
    }
}
