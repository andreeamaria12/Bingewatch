﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Personalized_movie_recommendation_system.Data;
using Personalized_movie_recommendation_system.Models;

namespace Personalized_movie_recommendation_system.Controllers
{
    public class MyListController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public MyListController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            
            user.Favorites = _context.FavoriteMovies.Where(fav => fav.UserId == user.Id).ToList();
            List<Movie> model = user.Favorites.Select(el => _context.Movies.Where(m => m.Id == el.MovieId).First()).ToList();

            return View(model);
        }
    }
}