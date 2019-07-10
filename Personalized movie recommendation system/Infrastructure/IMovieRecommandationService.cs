using Personalized_movie_recommendation_system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personalized_movie_recommendation_system.Infrastructure
{
    public interface IMovieRecommandationService
    {
        List<Movie> GetRecommendedMovies(User user);

        List<Movie> GetRecommendedMovies(User user, int genre);

        List<Movie> GetRecommendedMovies(User user, string genre);
    }
}
