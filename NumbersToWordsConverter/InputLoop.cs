using Conversions;
using Microsoft.Extensions.DependencyInjection;

internal class InputLoop {

    // wiring of components
    private static readonly ServiceProvider SERVICE_PROVIDER = new ServiceCollection()
            .AddSingleton<ToWordMapper, ToWordMapper>()
            .AddSingleton<INumberAsGroupsOf3Handler, NumberAsGroupsOf3Handler>()
            .AddSingleton<INumberToWordsConverter, NumberToWordsConverter>()
            .BuildServiceProvider();

    private static readonly ISet<string> KEYWORDS_TO_END_LOOP = new HashSet<string> { "exit", "quit" };

    private static void Main() {
        try {
            string userInput = GetUserInput();
            INumberToWordsConverter converter = SERVICE_PROVIDER.GetRequiredService<INumberToWordsConverter>();

            while (!KEYWORDS_TO_END_LOOP.Contains(userInput)) {
                string numberConvertedToWords = converter.ConvertNumberIntoWords(userInput);
                // present result of the conversion to the user
                Console.WriteLine(numberConvertedToWords);
                Console.WriteLine(string.Empty);
                
                userInput = GetUserInput();
            }
        }
        catch (ArgumentException e) {
            Console.WriteLine(string.Format("You entered an invalid input. See error message for details:\n'{0}'\nPlease try again by restarting the program.", e.Message));
        }
        catch (Exception e) {
            // unexpected error occurred
            Console.WriteLine(string.Format("Sorry, an unexpected error occurred:\n'{0}'\nPlease try again by restarting the program.", e));
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
