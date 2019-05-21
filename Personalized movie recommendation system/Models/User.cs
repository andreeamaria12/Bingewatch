using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Personalized_movie_recommendation_system.Models
{
    public class User : IdentityUser
    {
        public string Favorites_Json { get; set; }

        public string Watching_Json { get; set; }

        public string Watched_Json { get; set; }
    }

}
