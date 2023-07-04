using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Conversions {

    internal interface IToWordMapper {

        string ConvertDigitIntoWordOfSingleDigitNumbers(char digit, int numberOfDigitsInGroup);

        string ConvertDigitIntoWordOfTenMultiples(char digit);

        string ConvertNumberIntoIrregularlyConstructedWord(string number); 
    }

    internal class ToWordMapper : IToWordMapper {
        // text format strings for exception messages
        static readonly string EXC_MSG_CHAR_IS_NOT_A_DIGIT = "Cannot convert the given char '{0}' to a word {1}. Only digits (from 0 to 9) can be converted.";
        static readonly string EXC_MSG_NOT_A_SPECIAL_CASE_NUMBER = "The given number '{0}' is not a special case number which must be converted into a irregularly constructed word. Only numbers between '10' and '19' are special case numbers.";

        // constants
        static readonly string FRAGMENT_TYPE_SINGLE_DIGIT_NUMBERS = "of single digit numbers";
        static readonly string FRAGMENT_TYPE_TENS = "of ten-multiples";

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
            '5' => ConversionsConstants.W_FIVETY,
            '6' => ConversionsConstants.W_SIXTY,
            '7' => ConversionsConstants.W_SEVENTY,
            '8' => ConversionsConstants.W_EIGHTY,
            '9' => ConversionsConstants.W_NINETY,
            _ => throw new ArgumentException(string.Format(EXC_MSG_CHAR_IS_NOT_A_DIGIT, digit, FRAGMENT_TYPE_TENS))
        };

        public string ConvertNumberIntoIrregularlyConstructedWord(string number) => number switch {
            "10" => ConversionsConstants.W_TEN,
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
