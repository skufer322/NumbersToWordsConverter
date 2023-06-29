using System.Text.RegularExpressions;

namespace Conversions {

    internal class GlobalConstants {
        public static readonly char CH_ZERO = '0';
    }

    internal partial class NumbersToWordsConverter {
        // error strings / text format strings for exception messages
        static readonly string EXC_MSG_NUMBERS_STRING_IS_NULL = "The given numbers string is null or empty.";
        static readonly string EXC_MSG_INVALID_CHARS_TF = "The given numbers string '{0}' contains invalid characters! Only digits and the separator '{1}' are allowed.";
        static readonly string EXC_MSG_TOO_MANY_SEPARATORS_TF = "Too many separators in the given numbers string '{0}' ({1} separators), only 1 separator allowed at a max.";
        static readonly string EXC_MSG_MAX_NUMBER_OF_DOLLARS_EXCEEDED_TF = "The given number of dollars ('{0}') exceeds the allowed maximum number of dollars ('{1}').";
        static readonly string EXC_MSG_MAX_NUMBER_OF_CENTS_EXCEEDED_TF = "The given number of cents ('{0}') exceeds the allowed maximum number of cents ('{1}').";

        // constants 
        static readonly string SEPARATOR = ",";
        static readonly Regex REGEX_ALLOWED_CHARS = GenerateRegexForAllowedChars();
        static readonly Regex REGEX_WHITESPACES = GenerateRegexForWhitespaces();
        static readonly int MAX_DIGITS_DOLLARS = 9;
        static readonly int MAX_DIGITS_CENTS = 2;
        static readonly char CH_NINE = '9';
        static readonly int MAX_DIGITS_GROUP = 3;

        public static string ConvertNumbersToWords(string? numbers) {
            // check for invalid inputs
            if (string.IsNullOrEmpty(numbers)) {
                throw new ArgumentException(EXC_MSG_NUMBERS_STRING_IS_NULL);
            }
            if (!REGEX_ALLOWED_CHARS.IsMatch(numbers)) {
                throw new ArgumentException(string.Format(EXC_MSG_INVALID_CHARS_TF, numbers, SEPARATOR));
            }

            // split input into dollars and possibly cents
            string[] dollarsAndCents = numbers.Split(SEPARATOR);

            // check of there are too many separators
            if (dollarsAndCents.Length > 2) {
                int numberOfSeparators = dollarsAndCents.Length - 1;
                throw new ArgumentException(string.Format(EXC_MSG_TOO_MANY_SEPARATORS_TF, numbers, numberOfSeparators));
            }

            string dollars = SanitizeNumber(RemoveWhitespaces(dollarsAndCents[0]));
            if (dollars.Length > MAX_DIGITS_DOLLARS) {
                throw new ArgumentException(string.Format(EXC_MSG_MAX_NUMBER_OF_DOLLARS_EXCEEDED_TF, dollars, new String(CH_NINE, MAX_DIGITS_DOLLARS)));
            }

            string words = ConvertNumberIntoWords(dollars);
            words += words == "one" ? " dollar" : " dollars"; // TODO: auslagern (s. unten)
            if (dollarsAndCents.Length == 2) { // also account for cents
                string cents = SanitizeNumber(HandleCentInputWithOnlyOneDigit(RemoveWhitespaces(dollarsAndCents[1])));
                if (cents.Length > MAX_DIGITS_CENTS) {
                    throw new ArgumentException(string.Format(EXC_MSG_MAX_NUMBER_OF_CENTS_EXCEEDED_TF, cents, new String(CH_NINE, MAX_DIGITS_CENTS)));
                }
                string centsAsWords = ConvertNumberIntoWords(cents);
                centsAsWords += centsAsWords == "one" ? " cent" : " cents"; // TODO: mit gleicher Zeile für dollar in methode auslagern
                words += " and " + centsAsWords; // TODO: schöner machen?
            }

            return words;
        }

        [GeneratedRegex("^[0-9,\\s]+$")]
        private static partial Regex GenerateRegexForAllowedChars();

        [GeneratedRegex("\\s")]
        private static partial Regex GenerateRegexForWhitespaces();

        private static string SanitizeNumber(string number) {
            return NumberUtils.ReplaceEmptyStringWithZero(NumberUtils.TrimLeadingZeros(number));
        }

        private static string HandleCentInputWithOnlyOneDigit(string number) {
            return number.Length == MAX_DIGITS_CENTS ? number : number + GlobalConstants.CH_ZERO;
        }

        private static string RemoveWhitespaces(string number) {
            return REGEX_WHITESPACES.Replace(number, string.Empty);
        }

        private static string ConvertNumberIntoWords(string number) {
            string hundredsGroup = GetHundredsGroup(number);
            string thousandsGroup = GetThousandsGroup(number);
            string millionsGroup = GetMillionsGroups(number);

            string hgAsWord = ConvertNumberGroupIntoWord(hundredsGroup);
            string tgAsWord = ConvertNumberGroupIntoWord(thousandsGroup);
            string mgAsWord = ConvertNumberGroupIntoWord(millionsGroup);

            return string.Format("{0}{1}{2}", mgAsWord == string.Empty ? mgAsWord : mgAsWord + " million ", tgAsWord == string.Empty ? tgAsWord : tgAsWord + " thousand ", hgAsWord);
        }

        private static string GetHundredsGroup(string number) {
            return GetCharactersOfTrailingGroup(number);
        }

        private static string GetThousandsGroup(string number) {
            int numberOfCharactersToRemoveFromEnd = MAX_DIGITS_GROUP;
            // remove last 3 characters such that the thousands are the final group of number
            return number.Length > numberOfCharactersToRemoveFromEnd ? GetCharactersOfTrailingGroup(number[0..^numberOfCharactersToRemoveFromEnd]) : string.Empty;
        }

        private static string GetMillionsGroups(string number) {
            int numberOfCharactersToRemoveFromEnd = MAX_DIGITS_GROUP * 2;
            // remove last 6 characters such that the millions are the final group of number 
            return number.Length > numberOfCharactersToRemoveFromEnd ? GetCharactersOfTrailingGroup(number[0..^numberOfCharactersToRemoveFromEnd]) : string.Empty;
        }

        private static string GetCharactersOfTrailingGroup(string number) {
            int numberOfDigits = number.Length;
            return number.Substring(Math.Max(0, numberOfDigits - MAX_DIGITS_GROUP), Math.Min(numberOfDigits, MAX_DIGITS_GROUP));
        }

        private static string ConvertNumberGroupIntoWord(string numberGroup) {
            // TODO: prüfen, ob numberGroup.Length maximal 3 ist?
            if (numberGroup == string.Empty) {
                return string.Empty;
            }
            char[] digits = numberGroup.ToCharArray();
            Array.Reverse(digits);
            string lowestOrderDigitAsWordFragment = ConvertDigitIntoWord0to9(digits[0], digits.Length);
            string connector = digits[0] == GlobalConstants.CH_ZERO ? string.Empty : "-";
            string middleOrderDigitAsWordFragment = digits.Length >= MAX_DIGITS_GROUP - 1 ? ConvertDigitIntoWord0to90(digits[MAX_DIGITS_GROUP - 2]) + connector : string.Empty;
            string highestOrderDigitAsWordFragment = digits.Length == MAX_DIGITS_GROUP ? ConvertDigitIntoWord0to9(digits[MAX_DIGITS_GROUP - 1], digits.Length) + " hundred " : string.Empty;
            return string.Format("{0}{1}{2}", highestOrderDigitAsWordFragment, middleOrderDigitAsWordFragment, lowestOrderDigitAsWordFragment);
        }

        // TODO: auslagern in DigitToWordMapper o.ä.
        private static string ConvertDigitIntoWord0to9(char digit, int numberOfDigitsInGroup) => digit switch {
            '0' => numberOfDigitsInGroup == 1 ? "zero" : string.Empty,
            '1' => "one",
            '2' => "two",
            '3' => "three",
            '4' => "four",
            '5' => "five",
            '6' => "six",
            '7' => "seven",
            '8' => "eight",
            '9' => "nine",
            _ => throw new ArgumentException("TODO")
        };

        // TODO: auslagern in DigitToWordMapper o.ä.
        private static string ConvertDigitIntoWord0to90(char digit) => digit switch {
            '0' => string.Empty,
            '1' => "ten",
            '2' => "twenty",
            '3' => "thirty",
            '4' => "fourty",
            '5' => "fivety",
            '6' => "sixty",
            '7' => "seventy",
            '8' => "eighty",
            '9' => "ninety",
            _ => throw new ArgumentException("TODO")
        };
    }

    internal class NumberUtils {


        public static string TrimLeadingZeros(string number) {
            return number.TrimStart(GlobalConstants.CH_ZERO);
        }

        public static string ReplaceEmptyStringWithZero(string number) {
            return number == string.Empty ? Char.ToString(GlobalConstants.CH_ZERO) : number;
        }
    }
}
