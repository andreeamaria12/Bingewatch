using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Personalized_movie_recommendation_system.Controllers
{
    public class MyListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}