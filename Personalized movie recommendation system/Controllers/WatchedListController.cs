using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Personalized_movie_recommendation_system.Data;
using Personalized_movie_recommendation_system.Models;

namespace Personalized_movie_recommendation_system.Controllers
{
    public class WatchedListController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public WatchedListController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            user.WatchedMovies = _context.WatchedMovies.Where(w => w.UserId == user.Id).ToList();
            user.WatchedMovies ??= new List<WatchedMovie>();
            List<Movie> model = user.WatchedMovies.Select(el => _context.Movies.Where(m => m.Id == el.MovieId).First()).ToList();
            ViewBag.FavoriteMovies = _context.FavoriteMovies.Where(fav => fav.UserId == user.Id).ToList();
            return View(model);
        }
    }
}