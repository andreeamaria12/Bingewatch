using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personalized_movie_recommendation_system.Models
{
    public class UserViewModel
    {
        public List<User> Users { get; set; }
        public List<User> Admins { get; set; }
        public List<IdentityRole> Roles { get; set; }

    }
}
