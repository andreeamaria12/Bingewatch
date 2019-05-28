using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Personalized_movie_recommendation_system.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a movie title")]
        [Column(TypeName = "varchar(200)")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        [Column(TypeName = "varchar(max)")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter a release year")]
        public string ReleaseDate { get; set; }

        [Column(TypeName="decimal(3,2)")]
        public decimal Rating { get; set; }

        public int GenreId { get; set; }

        public  byte[] Image { get; set; }
        
        public int VoteCount { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string ImageUrl { get; set; }

        public string VideoUrl { get; set; }

        public ICollection<FavoriteMovie> UserFavorite { get; set; }

        public ICollection<WatchedMovie> UserWatchedMovie { get; set; }

    }
}
