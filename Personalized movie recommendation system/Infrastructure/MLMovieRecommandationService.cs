﻿using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using MovieRecommandationML;
using Personalized_movie_recommendation_system.Data;
using Personalized_movie_recommendation_system.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Personalized_movie_recommendation_system.Infrastructure
{
    public class MLMovieRecommandationService : IMovieRecommandationService
    {
        private readonly ApplicationDbContext _context;
        private MLContext _mlContext;
        private ITransformer _model;

        public MLMovieRecommandationService(ApplicationDbContext context)
        {
            _context = context;
        }

        private Task<List<Movie>> GetRecommendedMoviesAsync(User user)
        {
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");
            _mlContext = new MLContext();
            CreateCSV();
            IDataView trainingDataView = MovieRecommandationML.Core.LoadData(_mlContext);
            _model = MovieRecommandationML.Core.BuildAndTrainModel(_mlContext, trainingDataView);

            user.Favorites = _context.FavoriteMovies.Where(f => f.UserId == user.Id).ToList();
            user.Favorites ??= new List<FavoriteMovie>();
            user.WatchedMovies = _context.WatchedMovies.Where(w => w.UserId == user.Id).ToList();
            user.WatchedMovies ??= new List<WatchedMovie>();

            var movies = _context.Movies
               .Where(m => !user.Favorites.Select(o => o.MovieId).Contains(m.Id))
               .Where(m => !user.WatchedMovies.Select(o => o.MovieId).Contains(m.Id))
               .OrderByDescending(m => m.Rating).ToList();
            var predictions = movies.Select(m => (m, MovieRecommandationML.Core.UseModelForSinglePrediction(_mlContext, _model, new MovieRating() { movieId = m.Id, userId = user.Id })));
            return Task.FromResult(predictions.OrderByDescending(m => m.Item2).Select(m => m.m).ToList());
        }

        #region Interface implementation

        public List<Movie> GetRecommendedMovies(User user)
        {
            return GetRecommendedMoviesAsync(user).Result;
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

        #endregion

        #region ML


        private void CreateCSV()
        {
            var trainingDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-ratings-train.csv");
            if (File.Exists(trainingDataPath))
                File.Delete(trainingDataPath);
            using (StreamWriter sw = new StreamWriter(trainingDataPath))
            {
                _context.UserMovieRatings.ToList().ForEach(rating => 
                {
                    sw.Write(rating.UserId + "," + rating.MovieId + "," + rating.Rating + "\n");
                });
                sw.Close();
            }

        }


        #endregion
    }
}
