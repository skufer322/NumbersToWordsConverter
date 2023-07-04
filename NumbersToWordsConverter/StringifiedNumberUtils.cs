using System.Text.RegularExpressions;

namespace Conversions {

    internal partial class StringifiedNumberUtils {

        static readonly Regex REGEX_WHITESPACES = GenerateRegexForWhitespaces();

        [GeneratedRegex("\\s")]
        private static partial Regex GenerateRegexForWhitespaces();

        public static string RemoveWhitespaces(string number) {
            return REGEX_WHITESPACES.Replace(number, string.Empty);
        }

        public static string TrimLeadingZeros(string number) {
            return number.TrimStart(ConversionsConstants.CH_ZERO);
        }

        public static string ReplaceEmptyStringWithZero(string number) {
            return number == string.Empty ? char.ToString(ConversionsConstants.CH_ZERO) : number;
        }
    }
}
