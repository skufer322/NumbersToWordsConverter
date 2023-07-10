using System.Text.RegularExpressions;

namespace Conversions {

    /// <summary>
    /// Interface defining the methods for user input handlers.
    /// </summary>
    internal interface IInputHandler {

        /// <summary>
        /// Handles the user input. When handling the user input, the input is checked for its validity, is split into a unit and possibly a subunit number, both being sanitized for further processing.<br/><br/>
        /// The sanitization must include:
        /// <list type="bullet">
        /// <item><description>remove leading zeros of a number</description></item>
        /// <item><description>replace an empty string with "0"</description></item>
        /// <item><description>remove any whitespaces</description></item>
        /// <item><description>for one-digit subunit numbers, an '0' is appended (e.g. "1" turns to "10")</description></item>
        /// </list>
        /// </summary>
        /// <param name="number">number string to be processed (checked for validity, split into unit and possibly subunit, both being sanitized for further processing)</param>
        /// <returns>tuple containing sanitized unit (item 1) and possibly subunit (item 2) numbers</returns>
        /// <exception cref="ArgumentException">if the given number string is invalid or contains invalid symbols (allowed symbols are digits, one optional separator, and whitespaces)</exception>
        Tuple<string, string?> ProcessUserInput(string? number);
    }

    /// <summary>
    /// Implementation of <see cref="IInputHandler"/>.
    /// </summary>
    internal partial class InputHandler : IInputHandler {
        // error strings / text format strings for exception messages
        static readonly string EXC_MSG_NUMBER_STRING_IS_NULL_OR_EMPTY_OR_ONLY_WHITESPACE = "The given number string is null, empty, or only whitespace.";
        static readonly string EXC_MSG_INVALID_CHARS_TF = "The given number string '{0}' contains invalid characters! Only digits, a separator ('{1}'), and whitespaces are allowed.";
        static readonly string EXC_MSG_TOO_MANY_SEPARATORS_TF = "Too many separators in the given number string '{0}' ({1} separators), only 1 separator ('{2}') allowed at a max.";
        static readonly string EXC_MSG_TOO_MANY_DIGITS_FOR_SUBUNIT_TF = "The given subunit number ('{0}') has too many digits ({1}). For the subunit number, only {2} digits are allowed at a max.";

        // constants 
        static readonly string SEPARATOR = ",";
        static readonly Regex REGEX_ALLOWED_CHARS = GenerateRegexForAllowedChars();

        [GeneratedRegex("^[0-9,\\s]+$")]
        private static partial Regex GenerateRegexForAllowedChars();

        public Tuple<string, string?> ProcessUserInput(string? number) {
            // check for invalid inputs
            if (string.IsNullOrWhiteSpace(number)) {
                throw new ArgumentException(EXC_MSG_NUMBER_STRING_IS_NULL_OR_EMPTY_OR_ONLY_WHITESPACE);
            }
            if (!REGEX_ALLOWED_CHARS.IsMatch(number)) {
                throw new ArgumentException(string.Format(EXC_MSG_INVALID_CHARS_TF, number, SEPARATOR));
            }
            // split input into units and possibly subunits
            string[] unitsAndSubunits = number.Split(SEPARATOR);
            if (unitsAndSubunits.Length > 2) {
                // too many separators
                int numberOfSeparators = unitsAndSubunits.Length - 1;
                throw new ArgumentException(string.Format(EXC_MSG_TOO_MANY_SEPARATORS_TF, number, numberOfSeparators, SEPARATOR));
            }
            string sanitizedUnits = SanitizeNumber(StringifiedNumberUtils.RemoveWhitespaces(unitsAndSubunits[0]));
            string? sanitizedSubunits = unitsAndSubunits.Length == 2 ? SanitizeNumber(SpecialHandlingForSubunitInput(StringifiedNumberUtils.RemoveWhitespaces(unitsAndSubunits[1]))) : null;
            return new Tuple<string, string?>(sanitizedUnits, sanitizedSubunits);
        }

        private string SanitizeNumber(string number) {
            return StringifiedNumberUtils.ReplaceEmptyStringWithZero(StringifiedNumberUtils.TrimLeadingZeros(number));
        }

        private string SpecialHandlingForSubunitInput(string number) {
            if (number.Length == 1) {
                return number + ConversionsConstants.CH_0;
            }
            if (number.Length > ConversionsConstants.MAX_DIGITS_SUBUNIT) {
                throw new ArgumentException(string.Format(EXC_MSG_TOO_MANY_DIGITS_FOR_SUBUNIT_TF, number, number.Length, ConversionsConstants.MAX_DIGITS_SUBUNIT));
            }
            return number;
        }
    }
}
