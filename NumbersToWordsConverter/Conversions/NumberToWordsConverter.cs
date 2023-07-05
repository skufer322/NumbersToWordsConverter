using System.Text.RegularExpressions;

namespace Conversions {

    /// <summary>
    /// Interface defining the methods for implementations solving the problem of converting a number of a currency (dollars) into its word-based representation (natural language).
    /// </summary>
    internal interface INumberToWordsConverter {

        /// <summary>
        /// Converts the given number of a currency (dollars) into its word-based representation (natural language). The number can be of dollars and cents (separated by a separator symbol), or only of dollars (no separator).
        /// </summary>
        /// <param name="number">number to convert into its word-based representation</param>
        /// <returns>word-based representation of the given number</returns>
        /// <exception cref="ArgumentException">if the given number string contains invalid symbols (allowed symbols are digits, one optional separator, and whitespaces)</exception>
        string ConvertNumberIntoWords(string? number);
    }

    /// <summary>
    /// Implementation of <see cref="INumberToWordsConverter"/>.
    /// </summary>
    internal partial class NumberToWordsConverter : INumberToWordsConverter {
        // error strings / text format strings for exception messages
        static readonly string EXC_MSG_NUMBER_STRING_IS_NULL_OR_EMPTY = "The given number string is null, empty, or only whitespace.";
        static readonly string EXC_MSG_INVALID_CHARS_TF = "The given number string '{0}' contains invalid characters! Only digits, a separator ('{1}'), and whitespaces are allowed.";
        static readonly string EXC_MSG_TOO_MANY_SEPARATORS_TF = "Too many separators in the given number string '{0}' ({1} separators), only 1 separator ('{2}') allowed at a max.";
        static readonly string EXC_MSG_MAX_NUMBER_OF_DOLLARS_EXCEEDED_TF = "The given number of dollars ('{0}') exceeds the allowed maximum number of dollars ('{1}').";
        static readonly string EXC_MSG_MAX_NUMBER_OF_CENTS_EXCEEDED_TF = "The given number of cents ('{0}') exceeds the allowed maximum number of cents ('{1}').";

        // constants 
        static readonly string SEPARATOR = ",";
        static readonly Regex REGEX_ALLOWED_CHARS = GenerateRegexForAllowedChars();
        static readonly int MAX_DIGITS_DOLLARS = 9;
        static readonly int MAX_DIGITS_CENTS = 2;

        [GeneratedRegex("^[0-9,\\s]+$")]
        private static partial Regex GenerateRegexForAllowedChars();

        // class members
        private readonly INumberAsGroupsOf3Handler numberAsGroupsHandler;

        public NumberToWordsConverter(INumberAsGroupsOf3Handler numberAsGroupsHandler) {
            this.numberAsGroupsHandler = numberAsGroupsHandler;
        }

        public string ConvertNumberIntoWords(string? number) {
            // check for invalid inputs
            if (string.IsNullOrWhiteSpace(number)) {
                throw new ArgumentException(EXC_MSG_NUMBER_STRING_IS_NULL_OR_EMPTY);
            }
            if (!REGEX_ALLOWED_CHARS.IsMatch(number)) {
                throw new ArgumentException(string.Format(EXC_MSG_INVALID_CHARS_TF, number, SEPARATOR));
            }

            // split input into dollars and possibly cents
            string[] dollarsAndCents = number.Split(SEPARATOR);

            if (dollarsAndCents.Length > 2) {
                // too many separators
                int numberOfSeparators = dollarsAndCents.Length - 1;
                throw new ArgumentException(string.Format(EXC_MSG_TOO_MANY_SEPARATORS_TF, number, numberOfSeparators, SEPARATOR));
            }

            // account for dollars
            string dollars = SanitizeNumber(StringifiedNumberUtils.RemoveWhitespaces(dollarsAndCents[0]));
            if (dollars.Length > MAX_DIGITS_DOLLARS) {
                throw new ArgumentException(string.Format(EXC_MSG_MAX_NUMBER_OF_DOLLARS_EXCEEDED_TF, dollars, new string(ConversionsConstants.CH_NINE, MAX_DIGITS_DOLLARS)));
            }
            string words = ConvertPreprocessedNumberIntoWords(dollars);
            words = AddCurrencyWithCorrectCardinality(words, ConversionsConstants.DOLLAR, ConversionsConstants.DOLLARS);
            if (dollarsAndCents.Length == 2) {
                // also account for cents
                string cents = SanitizeNumber(HandleCentInputWithOnlyOneDigit(StringifiedNumberUtils.RemoveWhitespaces(dollarsAndCents[1])));
                if (cents.Length > MAX_DIGITS_CENTS) {
                    throw new ArgumentException(string.Format(EXC_MSG_MAX_NUMBER_OF_CENTS_EXCEEDED_TF, cents, new string(ConversionsConstants.CH_NINE, MAX_DIGITS_CENTS)));
                }
                string centsAsWords = ConvertPreprocessedNumberIntoWords(cents);
                centsAsWords = AddCurrencyWithCorrectCardinality(centsAsWords, ConversionsConstants.CENT, ConversionsConstants.CENTS);
                words = string.Format("{0} and {1}", words, centsAsWords);
            }
            return words;
        }

        private string SanitizeNumber(string number) {
            return StringifiedNumberUtils.ReplaceEmptyStringWithZero(StringifiedNumberUtils.TrimLeadingZeros(number));
        }

        private string ConvertPreprocessedNumberIntoWords(string number) {
            string hundredsGroup = numberAsGroupsHandler.GetHundredsGroup(number);
            string thousandsGroup = numberAsGroupsHandler.GetThousandsGroup(number);
            string millionsGroup = numberAsGroupsHandler.GetMillionsGroup(number);

            string hgAsWords = numberAsGroupsHandler.ConvertNumberGroupIntoWords(hundredsGroup);
            string tgAsWords = numberAsGroupsHandler.ConvertNumberGroupIntoWords(thousandsGroup);
            string mgAsWords = numberAsGroupsHandler.ConvertNumberGroupIntoWords(millionsGroup);

            return string.Format("{0}{1}{2}", numberAsGroupsHandler.GetGroupFragment(mgAsWords, ConversionsConstants.MILLION), numberAsGroupsHandler.GetGroupFragment(tgAsWords, ConversionsConstants.THOUSAND), hgAsWords);
        }

        private string AddCurrencyWithCorrectCardinality(string numberAsWords, string currencySingular, string currencyPlural) {
            return string.Format("{0} {1}", numberAsWords, numberAsWords == ConversionsConstants.W_ONE ? currencySingular : currencyPlural);
        }

        private string HandleCentInputWithOnlyOneDigit(string number) {
            return number.Length == 1 ? number + ConversionsConstants.CH_ZERO : number;
        }
    }
}
