using System;

namespace Helper
{
    class Input
    {
        public static char GetCharWithPrompt(string prompt, string errorMessage)
        {
            char userCharInput = ' ';
            bool ischar = false;
            if (!string.IsNullOrEmpty(prompt))
            {
                Console.WriteLine(prompt);
            }
            do
            {
                string input = Console.ReadLine();
                if(!string.IsNullOrEmpty(input) && input.Length == 1)
                {
                    userCharInput = input[0];
                    ischar = true;
                }
                else
                {
                    Console.WriteLine(errorMessage);
                }
            } while (!ischar);

            return userCharInput;
        }

        public static int GetIntWithPrompt(string prompt, string errorMessage)
        {
            bool conversionSuccessful = false;
            int userIntInput = 0;

            do
            {
                Console.Write(prompt);

                string? userInput = Console.ReadLine();

                if (userInput == null || userInput == "")
                {
                    Console.WriteLine(errorMessage);
                    continue;
                }
                conversionSuccessful = Int32.TryParse(userInput, out userIntInput);

                if (!conversionSuccessful)
                {
                    Console.WriteLine(errorMessage);
                }
            } while (!conversionSuccessful);

            return userIntInput;
        }

        public static double GetDoubleWithPrompt(string prompt, string errorMessage)
        {
            bool conversionSuccessful = false;
            double userDoubleInput = 0.0;

            do
            {
                Console.Write(prompt);

                string? userInput = Console.ReadLine();

                if (userInput == null || userInput == "")
                {
                    Console.WriteLine(errorMessage);
                    continue;
                }
                conversionSuccessful = Double.TryParse(userInput, out userDoubleInput);

                if (!conversionSuccessful)
                {
                    Console.WriteLine(errorMessage);
                }
            } while (!conversionSuccessful);

            return userDoubleInput;
        }

        public static string GetStringWithPrompt(string prompt, string errorMessage)
        {
            if(!string.IsNullOrEmpty(prompt))
            {
                Console.WriteLine(prompt);
            }
            do
            {


                string? userInput = Console.ReadLine();
                if (!String.IsNullOrEmpty(userInput))
                {
                    return userInput;
                }
                else
                {
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        Console.WriteLine(errorMessage);
                    }
                }

            } while (true);
            
        }

    }
}