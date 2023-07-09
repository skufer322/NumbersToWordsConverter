using ConverterProgram.Conversions;

namespace Conversions {

    /// <summary>
    /// Interface defining the methods for implementations solving the problem of converting a number-based representation of a currency into its word-based representation (natural language).
    /// </summary>
    internal interface INumberToWordsConverter {

        /// <summary>
        /// Converts the given number-based representation of a currency into its word-based representation (natural language). The number can be of currency units and sub-units (separated by a separator symbol), or only of currency units (no separator). 
        /// </summary>
        /// <param name="number">number-based representation to be converted into its word-based representation</param>
        /// <returns>word-based representation of the given number-based representation of the currency</returns>
        string ConvertNumberIntoWords(string? number);
    }

    /// <summary>
    /// Implementation of <see cref="INumberToWordsConverter"/>.
    /// </summary>
    internal partial class NumberToWordsConverter : INumberToWordsConverter {

        private readonly IInputHandler inputHandler;
        private readonly ACurrencyHandler currencyHandler;

        public NumberToWordsConverter(IInputHandler inputHandler, ACurrencyHandler currencyHandler) {
            this.inputHandler = inputHandler;
            this.currencyHandler = currencyHandler;
        }

        public string ConvertNumberIntoWords(string? number) {
            string[] unitsAndSubunits = inputHandler.ProcessUserInput(number);
            string units = unitsAndSubunits[0];
            string? subunits = unitsAndSubunits.Length == 1 ? null : unitsAndSubunits[1];
            return currencyHandler.ConvertCurrencyToWords(units, subunits);
        }
    }
}
