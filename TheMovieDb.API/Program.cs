using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Data;

namespace TheMovieDb.API
{
    class Program
    {
        static void Main(string[] args)
        {
            UpdateGenreDb();

            UpdateMovieDb();
        }

        static void UpdateGenreDb()
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Personalized_movie_recommendation_system_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(string.Format("SET IDENTITY_INSERT dbo.Genres ON")) { Connection = connection })
                {
                    command.ExecuteNonQuery();
                }
                using (SqlCommand command = new SqlCommand(string.Format("DELETE FROM dbo.Genres")) { Connection = connection })
                {
                    command.ExecuteNonQuery();
                }
                dynamic whatever = JsonConvert.DeserializeObject("{'genres':[{'id':28,'name':'Action'},{'id':12,'name':'Adventure'},{'id':16,'name':'Animation'},{'id':35,'name':'Comedy'},{'id':80,'name':'Crime'},{'id':99,'name':'Documentary'},{'id':18,'name':'Drama'},{'id':10751,'name':'Family'},{'id':14,'name':'Fantasy'},{'id':36,'name':'History'},{'id':27,'name':'Horror'},{'id':10402,'name':'Music'},{'id':9648,'name':'Mystery'},{'id':10749,'name':'Romance'},{'id':878,'name':'Science Fiction'},{'id':10770,'name':'TV Movie'},{'id':53,'name':'Thriller'},{'id':10752,'name':'War'},{'id':37,'name':'Western'}]}");
                for (int i = 0; i < 19; i++)
                {
                    using (SqlCommand command = new SqlCommand(string.Format("INSERT INTO dbo.Genres (Id, Name) values (@id, @name)")) { Connection = connection })
                    {
                        command.Parameters.AddWithValue("@id", Convert.ToInt32(whatever.genres[i].id));
                        command.Parameters.AddWithValue("@name", Convert.ToString(whatever.genres[i].name));
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        static void UpdateMovieDb()
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Personalized_movie_recommendation_system_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(string.Format("SET IDENTITY_INSERT dbo.Movies ON")) { Connection = connection })
                {
                    command.ExecuteNonQuery();
                }
                using (SqlCommand command = new SqlCommand(string.Format("DELETE FROM dbo.Movies")) { Connection = connection })
                {
                    command.ExecuteNonQuery();
                }
                for (int i = 1; i <= 19; i++)
                {
                    Console.WriteLine("Downloading page " + i + "...");
                    try
                    {
                        JsonConvert.DeserializeObject<Rootobject>((new System.Net.WebClient()).DownloadString("https://api.themoviedb.org/3/discover/movie?api_key=1b6317b113e3b3650be749f272d69cf8&language=en-US&sort_by=popularity.desc&include_adult=false&include_video=false&page=" + i)).results.ToList().ForEach(res =>
                        {
                            using (SqlCommand command = new SqlCommand(string.Format("INSERT INTO dbo.Movies (Id, Title, Description, ReleaseDate, Rating, GenreId, ImageUrl, VoteCount, Image, VideoUrl) VALUES (@id, @Title, @Description, @ReleaseDate, @Rating, @GenreId, @ImageUrl, @Votes, @Image, @VideoUrl)")) { Connection = connection })
                            {
                                command.Parameters.AddWithValue("@id", res.id);
                                command.Parameters.AddWithValue("@Title", res.title);
                                command.Parameters.AddWithValue("@Description", res.overview);
                                command.Parameters.AddWithValue("@ReleaseDate", res.release_date);
                                command.Parameters.AddWithValue("@Rating", res.vote_average);
                                command.Parameters.AddWithValue("@GenreId", (res.genre_ids.Length > 0) ? res.genre_ids[0] : -1);
                                command.Parameters.AddWithValue("@ImageUrl", res.poster_path);
                                command.Parameters.AddWithValue("@Votes", res.vote_count);


                                if (!File.Exists("temp" + res.poster_path))
                                    (new System.Net.WebClient()).DownloadFile("https://image.tmdb.org/t/p/w500" + res.poster_path, "temp" + res.poster_path);
                                byte[] data = System.IO.File.ReadAllBytes("temp" + res.poster_path);
                                command.Parameters.Add("@Image", SqlDbType.VarBinary, -1).Value = data;

                                try
                                {
                                    var v = JsonConvert.DeserializeObject<VideoRootObject.Rootobject>((new System.Net.WebClient()).DownloadString("https://api.themoviedb.org/3/movie/" + res.id + "/videos?api_key=1b6317b113e3b3650be749f272d69cf8&language=en-US")).results?.ToList().First();

                                    string url = "https://www." + v.site.ToLower() + ".com/embed/" + v.key;
                                    command.Parameters.AddWithValue("@VideoUrl", url);
                                }
                                catch
                                {
                                    Console.Write("*");
                                    command.Parameters.AddWithValue("@VideoUrl", string.Empty);
                                }
                              
                                try
                                {
                                    command.ExecuteNonQuery();
                                }
                                catch (Exception)
                                {
                                    Console.Write("*");
                                }
                            }
                        });

                    }
                    catch
                    {
                        Console.Write("+");
                    }
                }
            }
        }
    }
}
