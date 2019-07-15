using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Personalized_movie_recommendation_system.Data;
using Personalized_movie_recommendation_system.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            ViewBag.Genres = new SelectList(_context.Genres.Distinct(), "Id", "Name");
            Movie movie = _context.Movies.FirstOrDefault(m => m.Id == id);

            var model = new MovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                Rating = movie.Rating,
                GenreId = movie.GenreId,
                VoteCount = movie.VoteCount,
                ImageUrl = movie.ImageUrl,
                VideoUrl = movie.VideoUrl

            };

            return View(model);
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit(MovieViewModel model)
        {   
            if(ModelState.IsValid)
            {

                var movie = new Movie
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    ReleaseDate = model.ReleaseDate,
                    Rating = model.Rating,
                    GenreId = model.GenreId,
                    //Image = _context.Movies.FirstOrDefault(m => m.Id == model.Id).Image,
                    VoteCount = model.VoteCount,
                    ImageUrl = _context.Movies.FirstOrDefault(m => m.Id == model.Id).ImageUrl,
                    VideoUrl = model.VideoUrl

                };

                if(_context.Movies.Any(m => m.Id == movie.Id))
                {
                    movie.Image = _context.Movies.FirstOrDefault(m => m.Id == model.Id).Image;
                }

                if(model.Image != null && model.Image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.Image.CopyToAsync(memoryStream);
                        movie.Image = memoryStream.ToArray();
                        string path = Environment.CurrentDirectory + "\\wwwroot\\images\\" + model.Image.FileName;
                        model.Image.CopyTo(new FileStream(path, FileMode.Create));
                        movie.ImageUrl = path.Replace("\\", "/");
                    }
                }

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
                return View(model);
            }
        }

        public ViewResult Create()
        {
            ViewBag.Genres = new SelectList(_context.Genres.Distinct(), "Id", "Name");
            return View("Edit", new MovieViewModel());
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Movie movie = _context.Movies.FirstOrDefault(m => m.Id == Id);
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