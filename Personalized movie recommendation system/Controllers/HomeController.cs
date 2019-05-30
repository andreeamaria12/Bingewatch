using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Personalized_movie_recommendation_system.Data;
using Personalized_movie_recommendation_system.Infrastructure;
using Personalized_movie_recommendation_system.Models;

namespace Personalized_movie_recommendation_system.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieRecommandationService _popularMovieRecommandationService;
        private readonly ApplicationDbContext _context;

        public HomeController(IMovieRecommandationService popularMovieRecommandationService, ApplicationDbContext context)
        {
            _popularMovieRecommandationService = popularMovieRecommandationService;
            _context = context;
        }

        public IActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Genres = new SelectList(_context.Genres.Select(g => g.Name).Distinct());
                return View(_context.Movies);
            }
            else
            {
                return View();
            }
                
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
