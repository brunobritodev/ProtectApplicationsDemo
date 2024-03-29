using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ProtectApplication.Jwt.Data;

namespace ProtectApplication.Jwt.Configuration
{
    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(IServiceScope serviceScope)
        {
            var services = serviceScope.ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                await db.Database.EnsureCreatedAsync();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await EnsureSeedIdentityData(userManager, roleManager);
            }
        }

        /// <summary>
        /// Generate default admin user / role
        /// </summary>
        private static async Task EnsureSeedIdentityData(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {

            // Create admin role
            if (!await roleManager.RoleExistsAsync("Managers"))
            {
                var role = new IdentityRole("Managers");

                await roleManager.CreateAsync(role);
            }

            // Create admin user
            if (await userManager.FindByNameAsync("teste@teste.com") != null) return;

            var user = new IdentityUser()
            {
                UserName = "teste@teste.com",
                Email = "teste@teste.com",
                EmailConfirmed = true,
                LockoutEnd = null
            };

            var result = await userManager.CreateAsync(user, "Teste@123");

            if (result.Succeeded)
            {
                await userManager.AddClaimAsync(user, new Claim("EmployeeFunction", "Manager"));
                await userManager.AddClaimAsync(user, new Claim("Department", "Sales"));
                await userManager.AddToRoleAsync(user, "Managers");
            }
        }

    }
}
