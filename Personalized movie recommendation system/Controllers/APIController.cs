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
using Newtonsoft.Json;
using Personalized_movie_recommendation_system.Data;
using Personalized_movie_recommendation_system.Infrastructure;
using Personalized_movie_recommendation_system.Models;

namespace Personalized_movie_recommendation_system.Controllers
{
    [Route("api")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMovieRecommandationService _recommendationService;
        public APIController(ApplicationDbContext context, UserManager<User> userManager, IMovieRecommandationService recommendationService)
        {
            _context = context;
            _userManager = userManager;
            _recommendationService = recommendationService;
        }

        [Route("images")]
        public IActionResult GetImageAsync(string name, int w = -1, int h = -1)
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

            FavoriteMovie favoriteMovie = new FavoriteMovie
            {
                UserId = user.Id,
                User = user,
                MovieId = id,
                Movie = movie
            };
            _context.FavoriteMovies.Add(favoriteMovie);

            user.Favorites = _context.FavoriteMovies.Where(f => f.UserId == user.Id).ToList();
            user.Favorites ??= new List<FavoriteMovie>();
            user.Favorites.Add(favoriteMovie);

            movie.Favorites = _context.FavoriteMovies.Where(f => f.MovieId == movie.Id).ToList();
            movie.Favorites ??= new List<FavoriteMovie>();
            movie.Favorites.Add(favoriteMovie);

            await _userManager.UpdateAsync(user);

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

            FavoriteMovie favoriteMovie = user.Favorites.First(fav => fav.MovieId == id);
            user.Favorites.Remove(favoriteMovie);
            movie.Favorites.Remove(favoriteMovie);
            _context.FavoriteMovies.Remove(favoriteMovie);

            await _userManager.UpdateAsync(user);

            _context.SaveChanges();

            return Created("test", user.Favorites.Select(f => f.MovieId));
        }


        [Route("recommended")]
        [HttpGet]
        public async Task<IActionResult> GetRecommendedMoviesAsync(int startIndex = -1, int count = -1, string genre = "", string search = "")
        {
            if (startIndex == -1 || count == -1)
                return BadRequest();
            if (genre == null || genre == "All genres")
                genre = "";
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            var result = new List<Movie>();
            if (search != null && search != string.Empty)
                result = _context.Movies.Where(m => m.Title.ToUpper().Contains(search.ToUpper())).Distinct().ToList();
            else result = _recommendationService.GetRecommendedMovies(await _userManager.FindByNameAsync(User.Identity.Name), genre);
            if (count > result.Count)
                count = result.Count;
            try
            {
                result = result.GetRange(startIndex, count);
            }
            catch (Exception)
            {
                result = new List<Movie>();
            }
            
            result.ForEach(m =>
            {
                m.Image = null;
                m.Favorites = null;
            });
            return Content(JsonConvert.SerializeObject(result, serializerSettings));
        }

        [Route("AddWatchedMovie")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddWatchedMovie(int id)
        {
            if (!_context.Movies.Any(m => m.Id == id))
                return NotFound();
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            Movie movie = _context.Movies.Include(w => w.UserWatchedMovie)
                .Include(f => f.Favorites).First(m => m.Id == id);

            WatchedMovie watchedMovie = new WatchedMovie
            {
                UserId = user.Id,
                User = user,
                MovieId = id,
                Movie = movie
            };
            if(!_context.WatchedMovies.Any(w => w.MovieId == id))
            {
                _context.WatchedMovies.Add(watchedMovie);

                user.WatchedMovies ??= new List<WatchedMovie>();
                user.WatchedMovies.Add(watchedMovie);

                movie.UserWatchedMovie ??= new List<WatchedMovie>();
                movie.UserWatchedMovie.Add(watchedMovie);

                await _userManager.UpdateAsync(user);

                _context.SaveChanges();

                return Created("test", user.WatchedMovies.Select(w => w.MovieId));
            }
            else
            {
                return Created("test", "The movie is already in watched list");
            }
            
        }
    }
}