using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Personalized_movie_recommendation_system.Data;
using Personalized_movie_recommendation_system.Models;

namespace Personalized_movie_recommendation_system.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public MovieController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public ViewResult MoviePage(int id)
        {
            return View(_context.Movies.Where(movie => id == movie.Id).FirstOrDefault());
        }

        [Route("GetRating")]
        [HttpGet]
        public async Task<IActionResult> GetMovieRatingAsync(int id)
        {
            decimal result = 0;
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            user.UserMovieRatings = _context.UserMovieRatings.Where(r => r.UserId == user.Id).ToList();
            try
            {
                result = user.UserMovieRatings.Where(m => m.MovieId == id).FirstOrDefault().Rating;
            }
            catch
            {

            }
            return Content(result.ToString());
        }

         [Route("RateMovie")]
         [HttpPost]
         public async Task<IActionResult> RateMovieAsync(int id, string rating)
         {
            if (!_context.Movies.Any(m => m.Id == id))
                return NotFound();

            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            Movie movie = _context.Movies.Where(movie => id == movie.Id).FirstOrDefault();
            user.UserMovieRatings = _context.UserMovieRatings.Where(r => r.UserId == user.Id).ToList();
            movie.UserMovieRatings = _context.UserMovieRatings.Where(r => r.MovieId == movie.Id).ToList();

            UserMovieRating ratingMovie = new UserMovieRating
            {
                UserId = user.Id,
                User = user,
                MovieId = id,
                Movie = movie,
                Rating = decimal.Parse(rating)
            };

            if (_context.UserMovieRatings.Any(r => r.UserId == ratingMovie.UserId && r.MovieId == ratingMovie.MovieId))
            {
                _context.UserMovieRatings.Remove(_context.UserMovieRatings.FirstOrDefault(r => r.UserId == ratingMovie.UserId && r.MovieId == ratingMovie.MovieId));
                user.UserMovieRatings.Remove(user.UserMovieRatings.FirstOrDefault(r => r.UserId == ratingMovie.UserId && r.MovieId == ratingMovie.MovieId));
                movie.UserMovieRatings.Remove(movie.UserMovieRatings.FirstOrDefault(r => r.UserId == ratingMovie.UserId && r.MovieId == ratingMovie.MovieId));
            }

            _context.UserMovieRatings.Add(ratingMovie);

            user.UserMovieRatings ??= new List<UserMovieRating>();
            user.UserMovieRatings.Add(ratingMovie);

            movie.UserMovieRatings ??= new List<UserMovieRating>();
            movie.UserMovieRatings.Add(ratingMovie);

            await _userManager.UpdateAsync(user);

            _context.SaveChanges();

            return Created("test", user.UserMovieRatings.Select(r => r.Rating));
         }
    }
}