using Personalized_movie_recommendation_system.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personalized_movie_recommendation_system.Models
{
    public class GenreRepository : IGenreRepository
    {
        private AppDbContext context;

        public GenreRepository(AppDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Genre> Genres => context.Genres;
    }
}
