
using System;

namespace ApplicationTemplate.Services;

/// <summary>
///     You would need to inject your interfaces here to execute the methods in Invoke()
///     See the commented out code as an example
/// </summary>
public class MainService : IMainService
{
    private readonly IFileService _fileService;
    public MainService(IFileService fileService)
    {
        _fileService = fileService;
    }

    public void Invoke()
    {
        bool exit = false;
        int choice = 0;
       
        Console.WriteLine("Welcome to Blockbuster!");
        JsonMovie.ConvertMovies();                         //I spent a lot of time on the Csv project I didn't want to give up on it so I figured out how to convert the csv file to json
        do                                                 //I did cut it down to only 10 movies so when you read and write it doesn't scroll scroll scroll
                                                           //I did have some trouble with the interfacing and droped it, I understand it somewhat but the syntaxing of thing are still new to me
        {                                                  //Everything is broken out more modular but the plug and play is still a little fuzzy because I'm still a little stuck on grasping static and non static methods
            choice = JsonMovie.JsonMovieMenu();            //It works so I keep running with it but I know its going to hit a wall soon.

            if (choice == 1)
            {
                
                JsonMovie.Write();
            }
            else if (choice == 2)
            {
                JsonMovie.Read();
            }
            else if (choice == 3)
            {
                exit = true;
            }

        } while (!exit);
        Console.WriteLine();
        Console.WriteLine("Thank you for visiting Blockbuster, Good Bye!");
    }
}
