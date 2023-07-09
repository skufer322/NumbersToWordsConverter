namespace Conversions {

    /// <summary>
    /// Class holding constants used throughout the "Conversions" namespace. 
    /// </summary>
    internal class ConversionsConstants {

        // digits as chars
        public const char CH_0 = '0';
        public const char CH_1 = '1';
        public const char CH_2 = '2';
        public const char CH_3 = '3';
        public const char CH_4 = '4';
        public const char CH_5 = '5';
        public const char CH_6 = '6';
        public const char CH_7 = '7';
        public const char CH_8 = '8';
        public const char CH_9 = '9';

        // words for single digit numbers
        public const string W_ZERO = "zero";
        public const string W_ONE = "one";
        public const string W_TWO = "two";
        public const string W_THREE = "three";
        public const string W_FOUR = "four";
        public const string W_FIVE = "five";
        public const string W_SIX = "six";
        public const string W_SEVEN = "seven";
        public const string W_EIGHT = "eight";
        public const string W_NINE = "nine";

        // words for numbers 11 to 19
        public const string W_ELEVEN = "eleven";
        public const string W_TWELVE = "twelve";
        public const string W_THIRTEEN = "thirteen";
        public const string W_FOURTEEN = "fourteen";
        public const string W_FIFTEEN = "fifteen";
        public const string W_SIXTEEN = "sixteen";
        public const string W_SEVENTEEN = "seventeen";
        public const string W_EIGHTEEN = "eighteen";
        public const string W_NINETEEN = "nineteen";

        // words for numbers of ten-multiples from 10 to 90
        public const string W_TEN = "ten";
        public const string W_TWENTY = "twenty";
        public const string W_THIRTY = "thirty";
        public const string W_FOURTY = "fourty";
        public const string W_FIFTY = "fifty";
        public const string W_SIXTY = "sixty";
        public const string W_SEVENTY = "seventy";
        public const string W_EIGHTY = "eighty";
        public const string W_NINETY = "ninety";

        // all constants below could maybe be defined in a separate class, defined as "static readonly" instead of "const"
        // scale units
        public const string HUNDRED = "hundred";
        public const string THOUSAND = "thousand";
        public const string MILLION = "million";

        // currency units (singular & plural)
        public const string DOLLAR = "dollar";
        public const string DOLLARS = "dollars";
        public const string CENT = "cent";
        public const string CENTS = "cents";

        // max digits of unit and subunit
        public const int MAX_DIGITS_UNIT = 9;
        public const int MAX_DIGITS_SUBUNIT = 2;
    }
}
