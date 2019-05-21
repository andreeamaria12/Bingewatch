using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Personalized_movie_recommendation_system.Data;
using Personalized_movie_recommendation_system.Models;

namespace Personalized_movie_recommendation_system.Infrastructure
{
    public class PopularMovieRecommandationService : IMovieRecommandationService
    {
        private readonly ApplicationDbContext _context;

        public PopularMovieRecommandationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Movie> GetRecommendedMovies()
        {
            //TODO: base suggestions on user
            return _context.Movies.OrderByDescending(m => m.Rating).ToList();
        }
    }
}
