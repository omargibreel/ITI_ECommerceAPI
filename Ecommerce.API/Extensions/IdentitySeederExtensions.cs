using Ecommerce.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.API.Extensions
{
    public static class IdentitySeederExtensions
    {
        public static async Task SeedRolesAndUsersAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            await SeedUserAsync(
                userManager,
                email: "admin@gmail.com",
                password: "Admin123",
                firstName: "Admin",
                lastName: "System",
                role: "Admin");

            await SeedUserAsync(
                userManager,
                email: "user@gmail.com",
                password: "User123",
                firstName: "User",
                lastName: "Default",
                role: "User");
        }

        private static async Task SeedUserAsync(
            UserManager<AppUser> userManager,
            string email,
            string password,
            string firstName,
            string lastName,
            string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName
                };

                var createResult = await userManager.CreateAsync(user, password);
                if (!createResult.Succeeded)
                {
                    return;
                }
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
