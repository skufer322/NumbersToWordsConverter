using System.Text.RegularExpressions;

namespace Conversions {

    internal interface INumberToWordsConverter {
        string ConvertNumberIntoWords(string? number);
    }

    internal partial class NumberToWordsConverter : INumberToWordsConverter {
        // error strings / text format strings for exception messages
        static readonly string EXC_MSG_NUMBERS_STRING_IS_NULL_OR_EMPTY = "The given number string is null or empty.";
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
        private INumberAsGroupsOf3Handler numberAsGroupsHandler;

        public NumberToWordsConverter(INumberAsGroupsOf3Handler numberAsGroupsHandler) {
            this.numberAsGroupsHandler = numberAsGroupsHandler;
        }

        public string ConvertNumberIntoWords(string? number) {
            // check for invalid inputs
            if (string.IsNullOrEmpty(number)) {
                throw new ArgumentException(EXC_MSG_NUMBERS_STRING_IS_NULL_OR_EMPTY);
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

        private string HandleCentInputWithOnlyOneDigit(string number) {
            return number.Length == MAX_DIGITS_CENTS ? number : number + ConversionsConstants.CH_ZERO;
        }

        private string ConvertPreprocessedNumberIntoWords(string number) {
            string hundredsGroup = numberAsGroupsHandler.GetHundredsGroup(number);
            string thousandsGroup = numberAsGroupsHandler.GetThousandsGroup(number);
            string millionsGroup = numberAsGroupsHandler.GetMillionsGroups(number);

            string hgAsWord = numberAsGroupsHandler.ConvertNumberGroupIntoWord(hundredsGroup);
            string tgAsWord = numberAsGroupsHandler.ConvertNumberGroupIntoWord(thousandsGroup);
            string mgAsWord = numberAsGroupsHandler.ConvertNumberGroupIntoWord(millionsGroup);

            return string.Format("{0}{1}{2}", numberAsGroupsHandler.GetGroupFragment(mgAsWord, ConversionsConstants.MILLION), numberAsGroupsHandler.GetGroupFragment(tgAsWord, ConversionsConstants.THOUSAND), hgAsWord);
        }

        private string AddCurrencyWithCorrectCardinality(string numberAsWords, string currencySingular, string currencyPlural) {
            return string.Format("{0} {1}", numberAsWords, numberAsWords == ConversionsConstants.W_ONE ? currencySingular : currencyPlural);
        }
    }
}
