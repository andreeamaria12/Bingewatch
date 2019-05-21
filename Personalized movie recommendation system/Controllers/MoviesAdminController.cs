using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Personalized_movie_recommendation_system.Data;
using Personalized_movie_recommendation_system.Models;

namespace Personalized_movie_recommendation_system.Controllers
{
    [Authorize(Roles ="Admin")]
    public class MoviesAdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MoviesAdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ViewResult Index() => View(_context.Movies);

        public ViewResult Edit(int movieId)
        {
            ViewBag.Genres = new SelectList(_context.Genres.Select(g => g.Name).Distinct());
            return View(_context.Movies.FirstOrDefault(m => m.Id == movieId));
        }

        [HttpPost]
        public IActionResult Edit(Movie movie)
        {
            if(ModelState.IsValid)
            {
                TempData["message"] = $"{movie.Title} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                //there is smth wrong with the data values
                return View(movie);
            }
        }

        public ViewResult Create() => View("Edit", new Movie());

        [HttpPost]
        public IActionResult Delete(int movieId)
        {
            var movie = _context.Movies.Where(m => m.Id == movieId).First();
            if(movie != null)
            {
                TempData["message"] = $"{movie.Title} was deleted";
            }
            return RedirectToAction("Index");
        }
    }
}