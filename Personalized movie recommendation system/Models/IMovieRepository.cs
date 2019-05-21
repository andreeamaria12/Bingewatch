using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personalized_movie_recommendation_system.Models
{
    public interface IMovieRepository
    {
        IQueryable<Movie> Movies { get; }

        void SaveMovie(Movie movie);

        Movie DeleteMovie(int movieId);
    }
}
