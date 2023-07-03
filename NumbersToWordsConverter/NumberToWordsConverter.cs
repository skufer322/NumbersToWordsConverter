﻿using System.Text.RegularExpressions;

namespace Conversions {

    internal interface INumberToWordConverter {
        string ConvertNumberIntoWords(string? numbers);
    }

    internal partial class NumberToWordsConverter : INumberToWordConverter {
        // error strings / text format strings for exception messages
        static readonly string EXC_MSG_NUMBERS_STRING_IS_NULL = "The given numbers string is null or empty.";
        static readonly string EXC_MSG_INVALID_CHARS_TF = "The given numbers string '{0}' contains invalid characters! Only digits, a separator ('{1}'), and whitespaces are allowed.";
        static readonly string EXC_MSG_TOO_MANY_SEPARATORS_TF = "Too many separators in the given numbers string '{0}' ({1} separators), only 1 separator allowed at a max.";
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
        private INumbersAsGroupsOf3Handler numberGroupsHandler;

        public NumberToWordsConverter(INumbersAsGroupsOf3Handler numberGroupsHandler) {
            this.numberGroupsHandler = numberGroupsHandler;
        }

        public string ConvertNumberIntoWords(string? numbers) {
            // check for invalid inputs
            if (string.IsNullOrEmpty(numbers)) {
                throw new ArgumentException(EXC_MSG_NUMBERS_STRING_IS_NULL);
            }
            if (!REGEX_ALLOWED_CHARS.IsMatch(numbers)) {
                throw new ArgumentException(string.Format(EXC_MSG_INVALID_CHARS_TF, numbers, SEPARATOR));
            }

            // split input into dollars and possibly cents
            string[] dollarsAndCents = numbers.Split(SEPARATOR);

            // check if there are too many separators
            if (dollarsAndCents.Length > 2) {
                int numberOfSeparators = dollarsAndCents.Length - 1;
                throw new ArgumentException(string.Format(EXC_MSG_TOO_MANY_SEPARATORS_TF, numbers, numberOfSeparators));
            }

            // account for dollars
            string dollars = SanitizeNumber(StringifiedNumberUtils.RemoveWhitespaces(dollarsAndCents[0]));
            if (dollars.Length > MAX_DIGITS_DOLLARS) {
                throw new ArgumentException(string.Format(EXC_MSG_MAX_NUMBER_OF_DOLLARS_EXCEEDED_TF, dollars, new string(ConversionsConstants.CH_NINE, MAX_DIGITS_DOLLARS)));
            }

            string words = ConvertPreprocessedNumberIntoWords(dollars);
            words += GetCurrencyFragmentWithCorrectCardinality(words, ConversionsConstants.DOLLAR, ConversionsConstants.DOLLARS);
            if (dollarsAndCents.Length == 2) {
                // also account for cents
                string cents = SanitizeNumber(HandleCentInputWithOnlyOneDigit(StringifiedNumberUtils.RemoveWhitespaces(dollarsAndCents[1])));
                if (cents.Length > MAX_DIGITS_CENTS) {
                    throw new ArgumentException(string.Format(EXC_MSG_MAX_NUMBER_OF_CENTS_EXCEEDED_TF, cents, new string(ConversionsConstants.CH_NINE, MAX_DIGITS_CENTS)));
                }
                string centsAsWords = ConvertPreprocessedNumberIntoWords(cents);
                centsAsWords += GetCurrencyFragmentWithCorrectCardinality(centsAsWords, ConversionsConstants.CENT, ConversionsConstants.CENTS);
                words += " and " + centsAsWords; // TODO: schöner machen?
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
            string hundredsGroup = numberGroupsHandler.GetHundredsGroup(number);
            string thousandsGroup = numberGroupsHandler.GetThousandsGroup(number);
            string millionsGroup = numberGroupsHandler.GetMillionsGroups(number);

            string hgAsWord = numberGroupsHandler.ConvertNumberGroupIntoWord(hundredsGroup);
            string tgAsWord = numberGroupsHandler.ConvertNumberGroupIntoWord(thousandsGroup);
            string mgAsWord = numberGroupsHandler.ConvertNumberGroupIntoWord(millionsGroup);

            return string.Format("{0}{1}{2}", numberGroupsHandler.GetGroupFragment(mgAsWord, ConversionsConstants.MILLION), numberGroupsHandler.GetGroupFragment(tgAsWord, ConversionsConstants.THOUSAND), hgAsWord);
        }

        private string GetCurrencyFragmentWithCorrectCardinality(string numberAsWords, string currencySingular, string currencyPlural) {
            string fragmentWithCorrectCardinality = numberAsWords == ConversionsConstants.W_ONE ? currencySingular : currencyPlural;
            return string.Format(" {0}", fragmentWithCorrectCardinality);
        }
    }
}
