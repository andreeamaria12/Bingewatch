using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            Movie movie = _context.Movies.First(m => m.Id == id);

            FavoriteMovie favoriteMovie = new FavoriteMovie
            {
                UserId = user.Id,
                User = user,
                MovieId = id,
                Movie = movie
            };
            _context.FavoriteMovies.Add(favoriteMovie);

            user.Favorites = user.Favorites ?? new List<FavoriteMovie>();
            user.Favorites.Add(favoriteMovie);

            movie.UserFavorite = movie.UserFavorite ?? new List<FavoriteMovie>();
            movie.UserFavorite.Add(favoriteMovie);


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

            if (!user.Favorites.Any(fav => fav.MovieId == id))
                return NotFound();

            FavoriteMovie favoriteMovie = user.Favorites.First(fav => fav.MovieId == id);
            user.Favorites.Remove(favoriteMovie);
            _context.Movies.First(m => m.Id == id).UserFavorite.Remove(favoriteMovie);
            _context.FavoriteMovies.Remove(favoriteMovie);

            return Created("test", user.Favorites);
        }
    }
}