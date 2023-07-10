namespace Conversions {

    /// <summary>
    /// Interface defining the methods for handling numbers given as strings in groups of 3, nine digits at a max (i.e. millions, thousands, and hundreds), and converting a group into its word-based representation.
    /// </summary>
    internal interface INumberAsGroupsOf3Handler {

        /// <summary>
        /// From the given number string, returns the group of the three least significant digits (the "hundreds group").
        /// For example, if "123456789" is passed, "789" will be returned. If "12" is passed, "12" will be returned.
        /// </summary>
        /// <param name="number">number string from which the hundreds group is to be extracted</param>
        /// <returns>hundreds group of the number string</returns>
        string GetHundredsGroup(string number);

        /// <summary>
        /// From the given number string, returns the group of the four to six least significant digits (the "thousands group").
        /// For example, if "123456789" is passed, "456" will be returned. If "56789 is passed, "56" will be returned. If "789" is passed, an empty string will be returned (as there is no thousands group).
        /// </summary>
        /// <param name="number">number string from which the thousands group is to be extracted</param>
        /// <returns>thousands group of the number string, or an empty string if there is no thousands group</returns>
        string GetThousandsGroup(string number);

        /// <summary>
        /// From the given number string, returns the group of the seven to nine least significant digits (the "millions group").
        /// For example, if "123456789" is passed, "123" will be returned. If "23456789" is passed, "23" will be returned. If "456789" is passed, an empty string will be returned (as there is no millions group).
        /// </summary>
        /// <param name="number">number string from which the millions group is to be extracted</param>
        /// <returns>millions group of the number string, or an empty string if there is no millions group</returns>
        string GetMillionsGroup(string number);

        /// <summary>
        /// Converts the given number group of up to three digits into its word-based representation (natural language).
        /// </summary>
        /// <param name="numberGroup">number group which is to be converted into its word-based representation</param>
        /// <returns>number group which has been converted into its word-based representation</returns>
        /// <exception cref="ArgumentException">if the given group has a length greater than 3</exception>
        string ConvertNumberGroupIntoWords(string numberGroup);

        /// <summary>
        /// Returns the correctly formatted "group fragment", consisting of the word-based represenation of a number group and its scale (e.g. "million").
        /// </summary>
        /// <param name="numberAsWords">word-based representation of the number group for which the word group fragment is to be created</param>
        /// <param name="scale">scale of the number group for which the word group fragment is to be created</param>
        /// <param name="isWithSpaceAtFragmentEnd">whether the group fragment shall have a space character as its last character, or not</param>
        /// <returns>correctly formatted group fragment of the word-based represenation of a number group and its scale</returns>
        string GetGroupFragment(string numberAsWords, string scale, bool isWithSpaceAtFragmentEnd);
    }

    /// <summary>
    /// Implementation of <see cref="INumberAsGroupsOf3Handler"/>.
    /// </summary>
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
            string middleAndLowestOrderDigitsAsWords = (digits.Length >= 2 && digits[1] == ConversionsConstants.CH_1) // check for special cases 10 to 19
                ? toWordMapper.ConvertNumberIntoIrregularlyConstructedWord(numberGroup[^2..]) // special treatment for special cases
                : RegularlyConstructMiddleAndLowestOrderDigitsWords(digits); // regular treatment for non-special cases
            string highestOrderDigitAsWords = (digits.Length == MAX_DIGITS_GROUP)
                ? GetGroupFragment(toWordMapper.ConvertDigitIntoWordOfSingleDigitNumbers(digits[MAX_DIGITS_GROUP - 1], digits.Length), ConversionsConstants.HUNDRED, middleAndLowestOrderDigitsAsWords != string.Empty)
                : string.Empty;
            return string.Format("{0}{1}", highestOrderDigitAsWords, middleAndLowestOrderDigitsAsWords);
        }

        private string RegularlyConstructMiddleAndLowestOrderDigitsWords(char[] digits) {
            string lowestOrderDigitAsWord = toWordMapper.ConvertDigitIntoWordOfSingleDigitNumbers(digits[0], digits.Length);
            string connector = digits[0] == ConversionsConstants.CH_0 ? string.Empty : WORD_CONNECTOR;
            string middleOrderDigitAsWord = digits.Length >= 2 ? toWordMapper.ConvertDigitIntoWordOfTenMultiples(digits[1]) + connector : string.Empty;
            return string.Format("{0}{1}", middleOrderDigitAsWord, lowestOrderDigitAsWord);
        }

        public string GetGroupFragment(string numberAsWords, string scale, bool isWithSpaceAtFragmentEnd) {
            return numberAsWords == string.Empty ? string.Empty : string.Format("{0} {1}{2}", numberAsWords, scale, isWithSpaceAtFragmentEnd ? " " : string.Empty);
        }
    }
}
