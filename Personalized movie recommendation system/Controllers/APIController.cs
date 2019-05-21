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
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<int> favorites = JsonConvert.DeserializeObject<List<int>>(user.Favorites_Json);
            if (!favorites.Contains(id))
            {
                favorites.Add(id);
                user.Favorites_Json = JsonConvert.SerializeObject(favorites);
                await _userManager.UpdateAsync(user);
            }
            return Created("test", user.Favorites_Json);
        }

        [Route("RemoveFavorite")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveFavorite(int id)
        {
            if (!_context.Movies.Any(m => m.Id == id))
                return NotFound();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<int> favorites = JsonConvert.DeserializeObject<List<int>>(user.Favorites_Json);

            if(favorites.Contains(id))
            {
                favorites.Remove(id);
                user.Favorites_Json = JsonConvert.SerializeObject(favorites);

                await _userManager.UpdateAsync(user);
            }
            return Created("test", user.Favorites_Json);
        }
    }
}