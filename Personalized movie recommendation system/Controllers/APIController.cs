using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Personalized_movie_recommendation_system.Data;

namespace Personalized_movie_recommendation_system.Controllers
{
    [Route("api")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public APIController(ApplicationDbContext context)
        {
            _context = context;
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
    }
}