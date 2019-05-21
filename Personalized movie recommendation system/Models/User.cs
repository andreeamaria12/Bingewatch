using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personalized_movie_recommendation_system.Models
{
    public class User : IdentityUser
    {
        public List<Movie> Favorites { get; set; }
        public List<Movie> Watching { get; set; }
        public List<Movie> Watched { get; set; }
    }

}
