using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Personalized_movie_recommendation_system.Data;
using Personalized_movie_recommendation_system.Models;

namespace Personalized_movie_recommendation_system.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ViewResult MoviePage(int id)
        {
            return View(_context.Movies.Where(movie => id == movie.Id).FirstOrDefault());
        }
    }
}