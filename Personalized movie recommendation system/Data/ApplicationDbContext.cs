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

        public DbSet<FavoriteMovie> FavoriteMovies { get; set; }

        public DbSet<WatchedMovie> WatchedMovies { get; set; }

        public DbSet<UserMovieRating> UserMovieRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FavoriteMovie>()
                .HasKey(x => new { x.MovieId, x.UserId });
            builder.Entity<FavoriteMovie>()
                .HasOne(x => x.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(x => x.UserId);
            builder.Entity<FavoriteMovie>()
                .HasOne(x => x.Movie)
                .WithMany(m => m.Favorites)
                .HasForeignKey(x => x.MovieId);


            builder.Entity<WatchedMovie>()
                .HasKey(w => new { w.MovieId, w.UserId });
            builder.Entity<WatchedMovie>()
                .HasOne(x => x.User)
                .WithMany(u => u.WatchedMovies)
                .HasForeignKey(x => x.UserId);
            builder.Entity<WatchedMovie>()
                .HasOne(x => x.Movie)
                .WithMany(m => m.UserWatchedMovie)
                .HasForeignKey(x => x.MovieId);

            builder.Entity<UserMovieRating>()
                .HasKey(w => new { w.MovieId, w.UserId });
            builder.Entity<UserMovieRating>()
                .HasOne(x => x.User)
                .WithMany(u => u.UserMovieRatings)
                .HasForeignKey(x => x.UserId);
            builder.Entity<UserMovieRating>()
                .HasOne(x => x.Movie)
                .WithMany(m => m.UserMovieRatings)
                .HasForeignKey(x => x.MovieId);

        }

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
