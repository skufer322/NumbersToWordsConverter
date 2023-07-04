namespace Conversions {

    internal interface IDigitToWordMapper {
        string ConvertDigitIntoWordOfSingleDigitNumbers(char digit, int numberOfDigitsInGroup);
        string ConvertDigitIntoWordOfTens(char digit);
    }

    internal class DigitToWordMapper : IDigitToWordMapper {
        // text format strings for exception messages
        static readonly string EXC_MSG_CHAR_IS_NOT_A_DIGIT = "Cannot convert the given char '{0}' to a word {1}. Only digits (from 0 to 9) can be converted.";

        // constants
        static readonly string FRAGMENT_TYPE_SINGLE_DIGIT_NUMBERS = "of single digit numbers";
        static readonly string FRAGMENT_TYPE_TENS = "of tens";

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

        public string ConvertDigitIntoWordOfTens(char digit) => digit switch {
            '0' => string.Empty,
            '1' => ConversionsConstants.W_TEN,
            '2' => ConversionsConstants.W_TWENTY,
            '3' => ConversionsConstants.W_THIRTY,
            '4' => ConversionsConstants.W_FOURTY,
            '5' => ConversionsConstants.W_FIVETY,
            '6' => ConversionsConstants.W_SIXTY,
            '7' => ConversionsConstants.W_SEVENTY,
            '8' => ConversionsConstants.W_EIGHTY,
            '9' => ConversionsConstants.W_NINETY,
            _ => throw new ArgumentException(string.Format(EXC_MSG_CHAR_IS_NOT_A_DIGIT, digit, FRAGMENT_TYPE_TENS))
        };
    }
}
