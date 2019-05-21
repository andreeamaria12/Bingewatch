using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Personalized_movie_recommendation_system.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string Name { get; set; }

    }
}
