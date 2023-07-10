namespace Conversions {

    /// <summary>
    /// Interface defining the methods for implementations solving the problem of converting single digits into words of different scale or numbers into irregularly constructed words.
    /// </summary>
    internal interface IToWordMapper {

        /// <summary>
        /// Converts the given digit into its word-based representation of a single digit number. If the digit '0' is passed and this '0' is part of a number with more than one digit, an empty string is returned instead of the word "zero".
        /// </summary>
        /// <param name="digit">digit which is to be converted into its word-based representation of a single-digit number</param>
        /// <param name="numberOfDigitsInGroup">amount of digits the number to convert has</param>
        /// <returns>word-based representation of a single digit number for the passed digit, or an empty string if the digit '0' was passed and the '0' is part of a number with more than one digit</returns>
        /// /// <exception cref="ArgumentException">if the passed char is not a digit</exception>
        string ConvertDigitIntoWordOfSingleDigitNumbers(char digit, int numberOfDigitsInGroup);

        /// <summary>
        /// Converts the given digit into its word-based representation of a multiple of ten. If '0' is passed, an empty string is returned.
        /// </summary>
        /// <param name="digit">digit which is to be converted into its word-based representation of a multiple of ten</param>
        /// <returns>representation as a word of ten for the passed digit, or an empty string if '0' has been passed</returns>
        /// <exception cref="ArgumentException">if the passed char is not a digit</exception>
        string ConvertDigitIntoWordOfTenMultiples(char digit);

        /// <summary>
        /// Converts the given number string into its irregularly constructed word-based representation. Only the words for the numbers between "10" and "19" are irregularly constructed.
        /// </summary>
        /// <param name="number">number string which is to be converted into its irregularly constructed word-based representation</param>
        /// <returns>irregularly constructed word-based representation of the given number string</returns>
        /// <exception cref="ArgumentException">if any other strings than numbers between "10" and "19" are passed</exception>
        string ConvertNumberIntoIrregularlyConstructedWord(string number);
    }

    /// <summary>
    /// Implementation of <see cref="IToWordMapper"/>.
    /// </summary>
    internal class ToWordMapper : IToWordMapper {
        // text format strings for exception messages
        static readonly string EXC_MSG_CHAR_IS_NOT_A_DIGIT = "Cannot convert the given char '{0}' into a word of {1}. Only digits (from '0' to '9') can be converted.";
        static readonly string EXC_MSG_NOT_A_SPECIAL_CASE_NUMBER = "The given number '{0}' is not a special case number which has to be converted into a irregularly constructed word. Only numbers between '10' and '19' are special case numbers.";

        // constants
        static readonly string FRAGMENT_TYPE_SINGLE_DIGIT_NUMBERS = "single digit numbers";
        static readonly string FRAGMENT_TYPE_TEN_MULTIPLES = "ten-multiples";

        public string ConvertDigitIntoWordOfSingleDigitNumbers(char digit, int numberOfDigitsInGroup) => digit switch {
            ConversionsConstants.CH_0 => numberOfDigitsInGroup == 1 ? ConversionsConstants.W_ZERO : string.Empty,
            ConversionsConstants.CH_1 => ConversionsConstants.W_ONE,
            ConversionsConstants.CH_2 => ConversionsConstants.W_TWO,
            ConversionsConstants.CH_3 => ConversionsConstants.W_THREE,
            ConversionsConstants.CH_4 => ConversionsConstants.W_FOUR,
            ConversionsConstants.CH_5 => ConversionsConstants.W_FIVE,
            ConversionsConstants.CH_6 => ConversionsConstants.W_SIX,
            ConversionsConstants.CH_7 => ConversionsConstants.W_SEVEN,
            ConversionsConstants.CH_8 => ConversionsConstants.W_EIGHT,
            ConversionsConstants.CH_9 => ConversionsConstants.W_NINE,
            _ => throw new ArgumentException(string.Format(EXC_MSG_CHAR_IS_NOT_A_DIGIT, digit, FRAGMENT_TYPE_SINGLE_DIGIT_NUMBERS))
        };

        public string ConvertDigitIntoWordOfTenMultiples(char digit) => digit switch {
            ConversionsConstants.CH_0 => string.Empty,
            ConversionsConstants.CH_1 => ConversionsConstants.W_TEN,
            ConversionsConstants.CH_2 => ConversionsConstants.W_TWENTY,
            ConversionsConstants.CH_3 => ConversionsConstants.W_THIRTY,
            ConversionsConstants.CH_4 => ConversionsConstants.W_FOURTY,
            ConversionsConstants.CH_5 => ConversionsConstants.W_FIFTY,
            ConversionsConstants.CH_6 => ConversionsConstants.W_SIXTY,
            ConversionsConstants.CH_7 => ConversionsConstants.W_SEVENTY,
            ConversionsConstants.CH_8 => ConversionsConstants.W_EIGHTY,
            ConversionsConstants.CH_9 => ConversionsConstants.W_NINETY,
            _ => throw new ArgumentException(string.Format(EXC_MSG_CHAR_IS_NOT_A_DIGIT, digit, FRAGMENT_TYPE_TEN_MULTIPLES))
        };

        public string ConvertNumberIntoIrregularlyConstructedWord(string number) => number switch {
            "10" => ConversionsConstants.W_TEN, // is actually not an irregularly constructed word, but is also handled here for convenience
            "11" => ConversionsConstants.W_ELEVEN,
            "12" => ConversionsConstants.W_TWELVE,
            "13" => ConversionsConstants.W_THIRTEEN,
            "14" => ConversionsConstants.W_FOURTEEN,
            "15" => ConversionsConstants.W_FIFTEEN,
            "16" => ConversionsConstants.W_SIXTEEN,
            "17" => ConversionsConstants.W_SEVENTEEN,
            "18" => ConversionsConstants.W_EIGHTEEN,
            "19" => ConversionsConstants.W_NINETEEN,
            _ => throw new ArgumentException(string.Format(EXC_MSG_NOT_A_SPECIAL_CASE_NUMBER, number))
        };
    }
}
