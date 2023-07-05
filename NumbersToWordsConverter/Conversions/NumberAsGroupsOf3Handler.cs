namespace Conversions {

    internal interface INumberAsGroupsOf3Handler {

        string GetHundredsGroup(string number);

        string GetThousandsGroup(string number);

        string GetMillionsGroup(string number);

        string ConvertNumberGroupIntoWords(string numberGroup);

        string GetGroupFragment(string numberAsWords, string unit);
    }

    internal class NumberAsGroupsOf3Handler : INumberAsGroupsOf3Handler {

        // text format strings for exception messages
        static readonly string EXC_MSG_GROUP_TOO_LARGE_TF = "The number group '{0}' has too many digits. The maximum number of digits in a group is {1}.";

        // constants
        static readonly int MAX_DIGITS_GROUP = 3;
        static readonly string WORD_CONNECTOR = "-";

        // class members
        private readonly IToWordMapper toWordMapper;

        public NumberAsGroupsOf3Handler(IToWordMapper toWordMapper) {
            this.toWordMapper = toWordMapper;
        }

        public string GetHundredsGroup(string number) {
            return GetCharactersOfBackmostGroup(number);
        }

        public string GetThousandsGroup(string number) {
            int numberOfCharactersToRemoveFromEnd = MAX_DIGITS_GROUP; // remove last 3 characters such that the thousands are the backmost group of number
            return number.Length > numberOfCharactersToRemoveFromEnd ? GetCharactersOfBackmostGroup(number[0..^numberOfCharactersToRemoveFromEnd]) : string.Empty;
        }

        public string GetMillionsGroup(string number) {
            int numberOfCharactersToRemoveFromEnd = MAX_DIGITS_GROUP * 2; // remove last 6 characters such that the millions are the backmost group of number 
            return number.Length > numberOfCharactersToRemoveFromEnd ? GetCharactersOfBackmostGroup(number[0..^numberOfCharactersToRemoveFromEnd]) : string.Empty;
        }

        private static string GetCharactersOfBackmostGroup(string number) {
            int numberOfDigits = number.Length;
            return number.Substring(Math.Max(0, numberOfDigits - MAX_DIGITS_GROUP), Math.Min(numberOfDigits, MAX_DIGITS_GROUP));
        }

        public string ConvertNumberGroupIntoWords(string numberGroup) {
            // sanity check
            if (numberGroup.Length > MAX_DIGITS_GROUP) {
                throw new ArgumentException(string.Format(EXC_MSG_GROUP_TOO_LARGE_TF, numberGroup, MAX_DIGITS_GROUP));
            }

            if (numberGroup == string.Empty) {
                return string.Empty;
            }
            // create words from digits
            char[] digits = numberGroup.ToCharArray();
            Array.Reverse(digits);
            string middleAndLowestOrderDigitsAsWords = digits.Length >= 2 && digits[1] == ConversionsConstants.CH_ONE // check for special cases 10 to 19
                ? toWordMapper.ConvertNumberIntoIrregularlyConstructedWord(numberGroup[^2..]) // special treatment for special cases
                : RegularlyConstructMiddleAndLowestOrderDigitsWords(digits); // regular treatment for non-special cases
            string highestOrderDigitAsWords = digits.Length == MAX_DIGITS_GROUP ? GetGroupFragment(toWordMapper.ConvertDigitIntoWordOfSingleDigitNumbers(digits[MAX_DIGITS_GROUP - 1], digits.Length), ConversionsConstants.HUNDRED) : string.Empty;
            return string.Format("{0}{1}", highestOrderDigitAsWords, middleAndLowestOrderDigitsAsWords);
        }

        private string RegularlyConstructMiddleAndLowestOrderDigitsWords(char[] digits) {
            string lowestOrderDigitAsWord = toWordMapper.ConvertDigitIntoWordOfSingleDigitNumbers(digits[0], digits.Length);
            string connector = digits[0] == ConversionsConstants.CH_ZERO ? string.Empty : WORD_CONNECTOR;
            string middleOrderDigitAsWord = digits.Length >= MAX_DIGITS_GROUP - 1 ? toWordMapper.ConvertDigitIntoWordOfTenMultiples(digits[MAX_DIGITS_GROUP - 2]) + connector : string.Empty;
            return string.Format("{0}{1}", middleOrderDigitAsWord, lowestOrderDigitAsWord);
        }

        public string GetGroupFragment(string numberAsWords, string unit) {
            return numberAsWords == string.Empty ? string.Empty : string.Format("{0} {1} ", numberAsWords, unit);
        }
    }
}
