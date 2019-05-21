using Personalized_movie_recommendation_system.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personalized_movie_recommendation_system.Models
{
    public class EFMovieRepository : IMovieRepository
    {
        private AppDbContext context;

        public EFMovieRepository(AppDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Movie> Movies => context.Movies;

        public void SaveMovie(Movie movie)
        {
            if(movie.Id == 0)
            {
                context.Movies.Add(movie);
            }
            else
            {
                Movie dbEntry = context.Movies.FirstOrDefault(m => m.Id == movie.Id);
                if(dbEntry != null)
                {
                    dbEntry.Title = movie.Title;
                    dbEntry.Description = movie.Description;
                    dbEntry.Rating = movie.Rating;
                    dbEntry.GenreId = movie.GenreId;
                    dbEntry.Image = movie.Image;
                }
            }
            context.SaveChanges();
        }

        public Movie DeleteMovie(int movieId)
        {
            Movie dbEntry = context.Movies.FirstOrDefault(m => m.Id == movieId);
            if(dbEntry != null)
            {
                context.Movies.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
