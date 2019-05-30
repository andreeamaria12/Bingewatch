using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personalized_movie_recommendation_system.Data;
using Personalized_movie_recommendation_system.Models;

namespace Personalized_movie_recommendation_system.Controllers
{
    [Route("api")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public APIController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("images")]
        public IActionResult GetImageAsync(string name)
        {
            try
            {
                return File(_context.Movies.Where(m => m.ImageUrl == name).FirstOrDefault().Image, "image/jpeg");
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("AddFavorite")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddFavorite(int id)
        {
            if (!_context.Movies.Any(m => m.Id == id))
                return NotFound();
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            Movie movie = _context.Movies.Include(x => x.Favorites)
                .Include(w => w.UserWatchedMovie).First(m => m.Id == id);

            FavoriteMovieEntry favoriteMovie = new FavoriteMovieEntry
            {
                UserId = user.Id,
                User = user,
                MovieId = id,
                Movie = movie
            };
            _context.FavoriteMovies.Add(favoriteMovie);

            user.Favorites ??= new List<FavoriteMovieEntry>();
            user.Favorites.Add(favoriteMovie);

            movie.Favorites ??= new List<FavoriteMovieEntry>();
            movie.Favorites.Add(favoriteMovie);

            await _userManager.UpdateAsync(user);

            //await TryUpdateModelAsync(user);

            _context.SaveChanges();

            return Created("test", user.Favorites.Select(fav => fav.MovieId));
        }

        [Route("RemoveFavorite")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveFavorite(int id)
        {
            if (!_context.Movies.Any(m => m.Id == id))
                return NotFound();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            user.Favorites = _context.FavoriteMovies.Where(f => f.UserId == user.Id).ToList();
            if (user.Favorites == null || !user.Favorites.Any(fav => fav.MovieId == id))
                return NotFound();
                
            Movie movie = _context.Movies.First(m => m.Id == id);

            FavoriteMovieEntry favoriteMovie = user.Favorites.First(fav => fav.MovieId == id);
            user.Favorites.Remove(favoriteMovie);
            movie.Favorites.Remove(favoriteMovie);
            _context.FavoriteMovies.Remove(favoriteMovie);

            await _userManager.UpdateAsync(user);

            //await TryUpdateModelAsync(user);

            _context.SaveChanges();

            return Created("test", user.Favorites.Select(f => f.MovieId));
        }
    }
}