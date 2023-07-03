using Conversions;

internal class InputLoop {

    static readonly ISet<string> KEYWORDS_TO_END_LOOP = new HashSet<string> { "exit", "quit" };

    private static void Main() {

        try {
            string userInput = GetUserInput();

            while (!KEYWORDS_TO_END_LOOP.Contains(userInput)) {
                // pass user input to converter
                string convertedToWords = NumbersIntoWordsConverter.ConvertNumbersIntoWords(userInput);
                // present result of conversion to the user
                Console.WriteLine(convertedToWords);
                Console.WriteLine(string.Empty);
                userInput = GetUserInput();
            }
        }
        catch (ArgumentException e) {
            Console.WriteLine(string.Format("Sorry, you entered an invalid input. See error message for details:\n'{0}'\nPlease try again by restarting the program.", e.Message));
        }
        catch (Exception e) {
            // unexpected error occurred
            Console.WriteLine(string.Format("An unexpected error occurred:\n'{0}'\nPlease try again by restarting the program.", e));
        }
    }

    private static string GetUserInput() {
        // prompt user for input
        Console.Write("Enter currency number to convert into words: ");
        // read the user input
        string? userInput = Console.ReadLine();
        return userInput ?? string.Empty;
    }
}