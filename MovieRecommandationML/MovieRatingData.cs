using Microsoft.ML.Data;

namespace MovieRecommandationML
{
    public class MovieRating
    {
        [LoadColumn(0)]
        public string userId;
        [LoadColumn(1)]
        public float movieId;
        [LoadColumn(2)]
        public float Label;
    }

    public class MovieRatingPrediction
    {
        public float Label;
        public float Score;
    }
}
