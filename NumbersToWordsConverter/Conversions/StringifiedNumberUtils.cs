using System.Text.RegularExpressions;

namespace Conversions {

    /// <summary>
    /// Utility class for methods working on strings which represent numbers.
    /// </summary>
    internal partial class StringifiedNumberUtils {

        static readonly Regex REGEX_WHITESPACES = GenerateRegexForWhitespaces();

        [GeneratedRegex("\\s")]
        private static partial Regex GenerateRegexForWhitespaces();

        /// <summary>
        /// Removes all possible whitespaces from the given number string, and returns the cleaned-up string.
        /// </summary>
        /// <param name="number">number string from which all possible whitespaces are to be removed</param>
        /// <returns>number string from which all possible whitespaces have been removed</returns>
        public static string RemoveWhitespaces(string number) {
            return REGEX_WHITESPACES.Replace(number, string.Empty);
        }

        /// <summary>
        /// Removes all possible leading zeros from the given number string, and returns the cleaned-up string.
        /// </summary>
        /// <param name="number">number string from which all possible leading zeros are to be removed</param>
        /// <returns>number string from which all possible leading zeros have been removed</returns>
        public static string TrimLeadingZeros(string number) {
            return number.TrimStart(ConversionsConstants.CH_0);
        }

        /// <summary>
        /// Replaces the given number string with "0" if the given number string is empty. Else, the given number string is returned unchanged. 
        /// </summary>
        /// <param name="number">number string which is to be replaced with "0" if it is empty</param>
        /// <returns>"0" if the given number string is empty, or else the unchanged number string</returns>
        public static string ReplaceEmptyStringWithZero(string number) {
            return number == string.Empty ? char.ToString(ConversionsConstants.CH_0) : number;
        }
    }
}
