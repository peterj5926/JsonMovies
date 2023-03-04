
using ApplicationTemplate.Services;
using BetterConsoleTables;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ApplicationTemplate

{
    public class MovieClassMap : ClassMap<JsonMovie> 
    {
        public MovieClassMap()
        {
            Map(m => m.MovieId).Name("movieId");
            Map(m => m.ReleaseDate).Name("title").Convert(row =>    //I really wanted the year from the title string to match the example for the assignment so I used CsvHelper mapping to shred things apart a little more.
            {
                var columnValue = row.Row.GetField<string>("title");                
                int idx = columnValue.IndexOf('(');
                if (columnValue.Contains('('))
                {
                    do
                    {
                        columnValue = columnValue.Remove(0, idx + 1);                    
                        if (columnValue.IndexOf('(') != -1)
                        {
                            idx = columnValue.IndexOf('(');
                        }
                    } while (columnValue.IndexOf('(') != -1);
                    columnValue = columnValue?.Substring(0, 4);
                }
                else
                {
                    columnValue = "1900"; //default year
                }               
                DateTime columnValue1 = DateTime.Parse(columnValue + "-01-01"); // I wanted a real datetime for the json file and json needs more then the year to create the date
                return columnValue1 ;              
            });
            Map(m => m.Title).Name("title").Convert(row =>
            {
                var columnValue = row.Row.GetField<string>("title");
                return columnValue?.Substring(0, columnValue.Length - 6);
            });            
            Map(m => m.Genres).Name("genres").Convert(row =>
            {
                var columnValue = row.Row.GetField<string>("genres");
                return columnValue?.Split('|').ToArray();
            });
        }
    }
    public class JsonMovie 
    {
        [JsonProperty]
        
        public int MovieId { get; set; }
        [JsonProperty]
        public DateTime ReleaseDate { get; set; }
        [JsonProperty]
        
        public string Title { get; set; }
        //[JsonProperty]
        
        [Name("genres")]
        public string[] Genres { get; set; }         //If you use an array, the Json will list it differently when it writes it out, I was trying to follow the example to the T the best I could

        public JsonMovie()
        {

        }
        public JsonMovie(int movieId, DateTime releaseDate, string title, string[] genres)
        {
            MovieId = movieId;
            ReleaseDate = releaseDate;
            Title = title;
            Genres = genres;
        }
        public override string ToString()
        {
            return $"{MovieId},{ReleaseDate},{Title},{Genres}";
        }
        public static int JsonMovieMenu()
        {
            int choice = 0;
            Console.WriteLine();
            Console.WriteLine("1) Add a Movie to the list.");
            Console.WriteLine("2) Display the information about a movie.");
            Console.WriteLine("3) To exit.");
            choice = Input.GetIntWithPrompt("Select an option: ", "Please try again");  
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
        public static void Write()
        {
            string jfile = "jmovie.json";
            StreamReader sr = new StreamReader(jfile);
            string json1 = sr.ReadToEnd();
            List<JsonMovie> jsonMovies = JsonConvert.DeserializeObject<List<JsonMovie>>(json1);
            sr.Close();
           
            List<string> genre = new List<string>();
           
            int result = jsonMovies.Max(id => id.MovieId);
            int movieId = result + 1;
            Console.WriteLine();
            string title = Input.GetStringWithPrompt("Enter the movie title: ", "Please try again: ");
            var titlecheck = jsonMovies.FirstOrDefault(x => x.Title.Contains(title));

            if (titlecheck != null)
            {
                Console.WriteLine();
                Console.WriteLine("That Movie is already available");
            }
            else
            {
                Console.WriteLine();
                int year = Input.GetIntWithPrompt("What year was the movie released?: ", "That is not a valid year, please try again: ");
                DateTime releaseDate = DateTime.Parse(year + "-01-01");
                Console.WriteLine();
                GenreBuilder(genre);
                string[] genres = genre.ToArray();
                
                jsonMovies.Add(new JsonMovie(movieId, releaseDate, title, genres));
                string json = JsonConvert.SerializeObject(jsonMovies, Formatting.Indented);          //Theres an option for indenting so its not one long string
                Console.WriteLine($"{json}");                                                        // I printed out the file so when it hits the bottom you can see your new json was added
                Console.WriteLine();
                StreamWriter sw = new StreamWriter(jfile);
                sw.WriteLine($"{json}");
                sw.Close();
            }
        }
        public static void Read()                                                                   // I wanted to list the movies by title and let the user pick one to display all of its information
        {
            string jfile = "jmovie.json";
            StreamReader sr = new StreamReader(jfile);
            string json = sr.ReadToEnd();
            List<JsonMovie> jsonMovies = JsonConvert.DeserializeObject<List<JsonMovie>>(json);
            ListTitle(jsonMovies);
            
            int selection = Input.GetIntWithPrompt("Enter the number of the movie you'd like to see: ", "Please try again ");
            bool validSelection = false;
            do
            {
                if (selection > jsonMovies.Count || selection < 1)
                {
                    Console.WriteLine($"{selection} is not a valid selection, please try again ");
                    selection = Input.GetIntWithPrompt("Enter # for the movie you'd like to see: ", "Please try again ");
                }
                else
                {
                    int x = (selection - 1);
                    string g1 = string.Join(",",jsonMovies[x].Genres);
                    Console.WriteLine();
                    Console.WriteLine($"Movie ID:    {jsonMovies[x].MovieId}");
                    Console.WriteLine($"ReleaseDate: {jsonMovies[x].ReleaseDate}");
                    Console.WriteLine($"Movie Title: {jsonMovies[x].Title}");
                    Console.WriteLine($"Genres:      {g1}");
                    Console.WriteLine();
                   validSelection = true;

                   
                    
                }
            } while (!validSelection);
            sr.Close();

        }

        public static void ListTitle(List<JsonMovie> jsonMovies)
        {
            int count = 1;
            Console.WriteLine();
            foreach (JsonMovie movie in jsonMovies)
            {
                Console.WriteLine(count++ + "." + " " + movie.Title);
            }
            Console.WriteLine();
        }
        public static void GenreBuilder(List<string> genre)
        {
            
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

        public static void ConvertMovies()
        {
            string file = "movie10.csv";
            string jfileM = "jmovie.json";
           
            using (var streamReader = new StreamReader(file))
            {
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    csvReader.Context.RegisterClassMap<MovieClassMap>();
                    var jsonMovies = csvReader.GetRecords<JsonMovie>().ToList();
                    string json = JsonConvert.SerializeObject(jsonMovies, Formatting.Indented);
                    StreamWriter sw = new StreamWriter(jfileM);
                    sw.WriteLine(json);
                    sw.Close();
                }
            }
        }
    }

}




