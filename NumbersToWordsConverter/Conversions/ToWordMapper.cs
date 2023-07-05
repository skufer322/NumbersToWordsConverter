namespace Conversions {

    /// <summary>
    /// Interface defining the methods for implementations solving the problem of converting single digits into words of different scale or numbers into irregularly constructed words.
    /// </summary>
    internal interface IToWordMapper {

        /// <summary>
        /// Converts the given digit into its word-based representation of a single digit number. If the digit '0' is passed and this '0' is part of number with more than one digit, an empty string is returned instead of the word "zero".
        /// </summary>
        /// <param name="digit">digit which is to be converted into its word-based representation of a single-digit number</param>
        /// <param name="numberOfDigitsInGroup">amount of digits the number to be converted has</param>
        /// <returns>word-based representation of a single digit number for the passed digit, or an empty string if the digit '0' was passed and the '0' is part of a number with more than one digit</returns>
        string ConvertDigitIntoWordOfSingleDigitNumbers(char digit, int numberOfDigitsInGroup);

        /// <summary>
        /// Converts the given digit into its word-based representation of a multiple of ten. If '0' is passed, an empty string is returned.
        /// </summary>
        /// <param name="digit">digit which is to be converted into its word-based representation of a multiple of ten</param>
        /// <returns>word-based representation as a word of ten for the passed digit, or an empty string if '0' has been passed</returns>
        string ConvertDigitIntoWordOfTenMultiples(char digit);

        /// <summary>
        /// Converts the given number string into its irregularly constructed word-based representation. Only the words for the numbers "10" to "19" are irregularly constructed, other inputs will lead to an <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="number">number string which is to be connverted into its irregularly constructed word-based representation</param>
        /// <returns>irregularly constructed word-based representation of the given number string</returns>
        string ConvertNumberIntoIrregularlyConstructedWord(string number);
    }

    /// <summary>
    /// Implementation of <see cref="IToWordMapper"/>.
    /// </summary>
    internal class ToWordMapper : IToWordMapper {
        // text format strings for exception messages
        static readonly string EXC_MSG_CHAR_IS_NOT_A_DIGIT = "Cannot convert the given char '{0}' to a word of {1}. Only digits (from 0 to 9) can be converted.";
        static readonly string EXC_MSG_NOT_A_SPECIAL_CASE_NUMBER = "The given number '{0}' is not a special case number which has to be converted into a irregularly constructed word. Only numbers between '10' and '19' are special case numbers.";

        // constants
        static readonly string FRAGMENT_TYPE_SINGLE_DIGIT_NUMBERS = "single digit numbers";
        static readonly string FRAGMENT_TYPE_TENS = "ten-multiples";

        public string ConvertDigitIntoWordOfSingleDigitNumbers(char digit, int numberOfDigitsInGroup) => digit switch {
            '0' => numberOfDigitsInGroup == 1 ? "zero" : string.Empty,
            '1' => ConversionsConstants.W_ONE,
            '2' => ConversionsConstants.W_TWO,
            '3' => ConversionsConstants.W_THREE,
            '4' => ConversionsConstants.W_FOUR,
            '5' => ConversionsConstants.W_FIVE,
            '6' => ConversionsConstants.W_SIX,
            '7' => ConversionsConstants.W_SEVEN,
            '8' => ConversionsConstants.W_EIGHT,
            '9' => ConversionsConstants.W_NINE,
            _ => throw new ArgumentException(string.Format(EXC_MSG_CHAR_IS_NOT_A_DIGIT, digit, FRAGMENT_TYPE_SINGLE_DIGIT_NUMBERS))
        };

        public string ConvertDigitIntoWordOfTenMultiples(char digit) => digit switch {
            '0' => string.Empty,
            '1' => ConversionsConstants.W_TEN,
            '2' => ConversionsConstants.W_TWENTY,
            '3' => ConversionsConstants.W_THIRTY,
            '4' => ConversionsConstants.W_FOURTY,
            '5' => ConversionsConstants.W_FIFTY,
            '6' => ConversionsConstants.W_SIXTY,
            '7' => ConversionsConstants.W_SEVENTY,
            '8' => ConversionsConstants.W_EIGHTY,
            '9' => ConversionsConstants.W_NINETY,
            _ => throw new ArgumentException(string.Format(EXC_MSG_CHAR_IS_NOT_A_DIGIT, digit, FRAGMENT_TYPE_TENS))
        };

        public string ConvertNumberIntoIrregularlyConstructedWord(string number) => number switch {
            "10" => ConversionsConstants.W_TEN, // is actually not a irregularly constructed word, but is handled here for convenience
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
