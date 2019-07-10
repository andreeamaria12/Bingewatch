using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Personalized_movie_recommendation_system.Data;
using Personalized_movie_recommendation_system.Models;
using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace Personalized_movie_recommendation_system.Infrastructure
{
    public class PopularMovieRecommandationService : IMovieRecommandationService
    {
        private readonly ApplicationDbContext _context;

        public PopularMovieRecommandationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Movie> GetRecommendedMovies(User user)
        {
            MLContext mlContext = new MLContext();


            return _context.Movies
                .Include(x => x.Favorites)
                .Include(w => w.UserWatchedMovie)
                .OrderByDescending(m => m.Rating).ToList();
        }

        public List<Movie> GetRecommendedMovies(User user, int genre)
        {
            return _context.Movies
               .Include(x => x.Favorites)
               .Include(w => w.UserWatchedMovie)
               .OrderByDescending(m => m.Rating)
               .Where(m => m.GenreId == genre).ToList();
        }

        public List<Movie> GetRecommendedMovies(User user, string genre)
        {
            return (genre == string.Empty) ?
                GetRecommendedMovies(user)
                :
                GetRecommendedMovies(user, _context.Genres.Where(g => g.Name == genre).First().Id);
        }
    }
}
