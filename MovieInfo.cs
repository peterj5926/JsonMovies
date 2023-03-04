using BetterConsoleTables;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTemplate
{
    public class MovieInfo
    {
        [Name("movieId")]
        public int MovieId { get; set; }
        [Name("title")]
        public string Title { get; set; }
        [Name("genres")]
        public string Genres { get; set; }

        public MovieInfo()
        {

        }
        public MovieInfo(int movieId, string title, string genres)
        {
            MovieId = movieId;
            Title = title;
            Genres = genres;
        }
        public override string ToString()
        {
            return $"{MovieId},{Title},{Genres}";
        }

        public static int MovieMenu()
        {
            int choice = 0;
            Console.WriteLine("1) Add a Movie to the list.");
            Console.WriteLine("2) To see a list of all Movies.");
            Console.WriteLine("3) To exit.");
            choice = Input.GetIntWithPrompt("Select an option: ", "Please try again");  // we created these input methods in intro that come in handy for user management
            do
            {
                if (choice > 3 || choice < 1)
                {
                    Console.WriteLine("Please select a menu option");
                    choice = Input.GetIntWithPrompt("Select an option: ", "Please try again");
                }
            } while (choice > 5 || choice < 1);

            return choice;
        }

        public static void Read()
        {
            string file = "movies.csv";
            int maxIndex = 0;
            int i = 0;
            int j = 1000;
            bool done = false;
            using (var streamReader = new StreamReader(file))
            {
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    var records = csvReader.GetRecords<MovieInfo>().ToList();
                    do
                    {
                        done = false;
                        maxIndex = (records.Count);
                        List<MovieInfo> movielist = records.GetRange(i, j);
                        Table table = new Table(TableConfiguration.Unicode());
                        table.From<MovieInfo>(movielist);
                        Console.Write(table.ToString());

                        if ((i + j) < (maxIndex - j))
                        {

                            i += j;
                            Console.WriteLine("Press Enter For More Movies: ");                                                   
                            Console.ReadLine();                                                                                  
                        }                                                                                                       
                        else if ((i + j) > (maxIndex - j))
                        {
                            i += j;
                            j = (maxIndex - i);
                            if (j == 0)
                            {
                                Console.WriteLine("Shows Over!");
                                i = 0;                                                                                      
                                j = 1000;
                                done = true;
                            }
                        }

                    } while (!done);



                }
            }
        }
        public static void AddMovie()
        {
            string file = "movies.csv";
            StreamReader sr = new StreamReader(file);
            List<string> genre = new List<string>();
            List<UInt64> MovieIds = new List<UInt64>();
            List<string> MovieTitles = new List<string>();
            List<string> MovieGenres = new List<string>();                                    

            sr.ReadLine();

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                int idx = line.IndexOf('"');
                if (idx == -1)
                {
                    string[] movieDetails = line.Split(',');
                    MovieIds.Add(UInt64.Parse(movieDetails[0]));
                    MovieTitles.Add(movieDetails[1]);
                    MovieGenres.Add(movieDetails[2].Replace("|", ", "));
                }
                else
                {
                    MovieIds.Add(UInt64.Parse(line.Substring(0, idx - 1)));
                    line = line.Substring(idx + 1);
                    idx = line.IndexOf('"');
                    MovieTitles.Add(line.Substring(0, idx));
                    line = line.Substring(idx + 2);
                    MovieGenres.Add(line.Replace("|", ", "));
                }
            }
            sr.Close();
            string movietitle = Input.GetStringWithPrompt("Enter the movie title: ", "Please try again: ");
            List<string> LowerCaseTitle = MovieTitles.ConvertAll(t => t.ToLower());
            if (LowerCaseTitle.Contains(movietitle.ToLower()))
            {
                Console.WriteLine("That movie name already exist");
            }
            else
            {
                UInt64 movid = MovieIds.Max() + 1;
                MovieInfo.GenreBuilder(genre);
                string genresString = string.Join("|", genre);
                movietitle = movietitle.IndexOf(',') != -1 ? $"\"{movietitle}\"" : movietitle;
                Console.WriteLine($"{movid},{movietitle},{genresString}");
                StreamWriter sw = new StreamWriter(file, true);
                sw.WriteLine($"{movid},{movietitle},{genresString}");                                                           
                sw.Close();
                MovieIds.Add(movid);
                MovieTitles.Add(movietitle);
                MovieGenres.Add(genresString);
                genre.Clear();                                                                                                   


            }

        }
        public static void GenreBuilder(List<string> genre)
        {
            //List<string> genre = new List<string>(); why does this make it angry
            string userInput;
            string genres;

            userInput = Input.GetStringWithPrompt("Are there any Genres? Y/N: ", "Please try again");
            do
            {

                if (userInput.ToUpper() == "Y")
                {
                    genres = Input.GetStringWithPrompt("Please add Genre", "Please try again");
                    genre.Add(genres);
                    userInput = Input.GetStringWithPrompt("Are there any more Genres? Y/N: ", "Please try again");
                }
                else if (userInput.ToUpper() == "N" && genre.Count == 0)
                {
                    genre.Add("(no genres listed)");
                }
                else
                {
                    Console.WriteLine("Please select Y or N ");
                    userInput = "Y";
                }
            } while (userInput.ToUpper() == "Y");
        }
    }
}
