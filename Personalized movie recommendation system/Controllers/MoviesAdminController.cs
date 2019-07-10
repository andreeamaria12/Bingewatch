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

        [Route("Edit")]
        [HttpGet]
        public ViewResult Edit(int id)
        {
            ViewBag.Genres = new SelectList(_context.Genres.Select(g => g.Name).Distinct());
            Movie movie = _context.Movies.FirstOrDefault(m => m.Id == id);

            return View(movie);
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit(Movie movie)
        {
            string genreName = Request.Form["GenreId"].ToString();
            movie.GenreId = _context.Genres.FirstOrDefault(g => g.Name == genreName).Id;
           
            if (ModelState.IsValid)
            {

                if (_context.Movies.Any(m => m.Id == movie.Id))
                {
                    _context.Movies.Remove(_context.Movies.FirstOrDefault(m => m.Id == movie.Id));
                }
                
                _context.Movies.Add(movie);
                _context.SaveChanges();
                TempData["message"] = $"{movie.Title} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                return View(movie);
            }
        }

        public ViewResult Create()
        {
            ViewBag.Genres = new SelectList(_context.Genres.Select(g => g.Name).Distinct());
            return View("Edit");
        }

        [HttpPost]
        public IActionResult Delete(int movieId)
        {
            Movie movie = _context.Movies.FirstOrDefault(m => m.Id == movieId);
            if(movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
                TempData["message"] = $"{movie.Title} was deleted";
            }
            return RedirectToAction("Index");
        }
    }
}