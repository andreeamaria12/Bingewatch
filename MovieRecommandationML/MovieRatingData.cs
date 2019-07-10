using Microsoft.ML.Data;

namespace MovieRecommandationML
{
    // <SnippetMovieRatingClass>
    public class MovieRating
    {
        [LoadColumn(0)]
        public string userId;
        [LoadColumn(1)]
        public float movieId;
        [LoadColumn(2)]
        public float Label;
    }
    // </SnippetMovieRatingClass>

    // <SnippetPredictionClass>
    public class MovieRatingPrediction
    {
        public float Label;
        public float Score;
    }
    // </SnippetPredictionClass>
}
