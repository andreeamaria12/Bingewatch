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

        [Required(ErrorMessage = "Please enter a release date")]
        public string ReleaseDate { get; set; }

        [Required(ErrorMessage = "Please enter a rating")]
        [Column(TypeName="decimal(3,2)")]
        public decimal Rating { get; set; }

        [Required(ErrorMessage = "Please specify a genre")]
        public int GenreId { get; set; }

        //[Required(ErrorMessage = "Please insert a cover image")]
        public  byte[] Image { get; set; }
        
        public int VoteCount { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Please specify a video path")]
        public string VideoUrl { get; set; }

        public ICollection<FavoriteMovie> Favorites { get; set; }

        public ICollection<WatchedMovie> UserWatchedMovie { get; set; }

        public ICollection<UserMovieRating> UserMovieRatings { get; set; }

    }
}
