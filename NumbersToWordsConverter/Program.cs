using Conversions;

internal class Program
{
    private static void Main()
    {

        try
        {
            string? userInput = GetUserInput();

            while (userInput != "quit" && userInput != "exit") {
                // pass user input to converter
                string convertedToWords = NumbersToWordsConverter.ConvertNumbersToWords(userInput);

                // present result of conversion to the user
                Console.WriteLine(convertedToWords);
                Console.WriteLine(String.Empty);

                userInput = GetUserInput();
            }

        }
        catch (ArgumentException e)
        {
            Console.WriteLine(String.Format("Sorry, you entered an invalid input. See error message for details:\n'{0}'\nPlease try again.", e.Message));
        }
        catch (Exception e)
        {
            // TODO: sth went wrong
            Console.WriteLine("Something went wrong!!! " + e);
        }
    }

    private static string? GetUserInput() {
        // prompt user for input
        Console.Write("Enter currency number to convert into words: ");
        // read the user input
        return Console.ReadLine();
    }
}