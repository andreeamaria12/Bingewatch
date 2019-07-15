using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace MovieRecommandationML
{
    public class Core
    {

        public static IDataView LoadData(MLContext mlContext)
        {
            // Load training dataset using datapath
            var trainingDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-ratings-train.csv");

            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<MovieRating>(trainingDataPath, hasHeader: false, separatorChar: ',');

            return trainingDataView;
        }

        // Build and train model
        public static ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainingDataView)
        {
            // Add data transformations
            IEstimator<ITransformer> estimator = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "userIdEncoded", inputColumnName: "userId")
                .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "movieIdEncoded", inputColumnName: "movieId"));

            // Set algorithm options and append algorithm
            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "userIdEncoded",
                MatrixRowIndexColumnName = "movieIdEncoded",
                LabelColumnName = "Label",
                NumberOfIterations = 20,
                ApproximationRank = 100
            };

            var trainerEstimator = estimator.Append(mlContext.Recommendation().Trainers.MatrixFactorization(options));

            Console.WriteLine("=============== Training the model ===============");
            ITransformer model = trainerEstimator.Fit(trainingDataView);

            return model;
        }

        // Use model for single prediction
        public static float UseModelForSinglePrediction(MLContext mlContext, ITransformer model, MovieRating m)
        {
            Console.WriteLine("=============== Making a prediction ===============");
            var predictionEngine = mlContext.Model.CreatePredictionEngine<MovieRating, MovieRatingPrediction>(model);

            // Create test input & make single prediction
            var testInput = new MovieRating { userId = m.userId, movieId = m.movieId };

            var movieRatingPrediction = predictionEngine.Predict(testInput);

            return movieRatingPrediction.Score;
        }
    }
}
