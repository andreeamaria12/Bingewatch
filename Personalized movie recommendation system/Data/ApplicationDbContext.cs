using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Personalized_movie_recommendation_system.Models;

namespace Personalized_movie_recommendation_system.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public static async Task CreateAdminAccountAndRoles(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string adminUsername = configuration["Data:AdminUser:Name"];
            string adminEmail = configuration["Data:AdminUser:Email"];
            string adminPassword = configuration["Data:AdminUser:Password"];
            string adminRole = configuration["Data:AdminUser:Role"];
            string userRole = configuration["Data:UserRole"];

            if (await userManager.FindByNameAsync(adminUsername) == null)
            {
                if (await roleManager.FindByNameAsync(adminRole) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(adminRole));
                }

                User adminUser = new User
                {
                    UserName = adminUsername,
                    Email = adminEmail
                };

                IdentityResult result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }

            if (await roleManager.FindByNameAsync(userRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(userRole));
            }
        }
    }
}
