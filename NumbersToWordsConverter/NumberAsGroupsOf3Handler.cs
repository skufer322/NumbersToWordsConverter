using System.Linq.Expressions;

namespace Conversions {

    internal interface INumberAsGroupsOf3Handler {
        string GetHundredsGroup(string number);

        string GetThousandsGroup(string number);

        string GetMillionsGroups(string number);

        string ConvertNumberGroupIntoWord(string numberGroup);

        string GetGroupFragment(string numberAsWords, string unit);
    }

    internal class NumberAsGroupsOf3Handler : INumberAsGroupsOf3Handler {

        // text format strings for exception messages
        static readonly string EXC_MSG_GROUP_TOO_LARGE_TF = "The number group '{0}' has too many digits. The maximum number of digits in a group is {1}.";

        // constants
        static readonly int MAX_DIGITS_GROUP = 3;
        static readonly string DASH_CONNECTOR = "-";

        // class members
        private IDigitToWordMapper wordMapper;

        public NumberAsGroupsOf3Handler(IDigitToWordMapper wordMapper) {
            this.wordMapper = wordMapper;
        }

        public string GetHundredsGroup(string number) {
            return GetCharactersOfTrailingGroup(number);
        }

        public string GetThousandsGroup(string number) {
            int numberOfCharactersToRemoveFromEnd = MAX_DIGITS_GROUP;
            // remove last 3 characters such that the thousands are the final group of number
            return number.Length > numberOfCharactersToRemoveFromEnd ? GetCharactersOfTrailingGroup(number[0..^numberOfCharactersToRemoveFromEnd]) : string.Empty;
        }

        public string GetMillionsGroups(string number) {
            int numberOfCharactersToRemoveFromEnd = MAX_DIGITS_GROUP * 2;
            // remove last 6 characters such that the millions are the final group of number 
            return number.Length > numberOfCharactersToRemoveFromEnd ? GetCharactersOfTrailingGroup(number[0..^numberOfCharactersToRemoveFromEnd]) : string.Empty;
        }

        private static string GetCharactersOfTrailingGroup(string number) {
            int numberOfDigits = number.Length;
            return number.Substring(Math.Max(0, numberOfDigits - MAX_DIGITS_GROUP), Math.Min(numberOfDigits, MAX_DIGITS_GROUP));
        }

        public string ConvertNumberGroupIntoWord(string numberGroup) {
            if (numberGroup.Length > MAX_DIGITS_GROUP) {
                throw new ArgumentException(string.Format(EXC_MSG_GROUP_TOO_LARGE_TF, numberGroup, MAX_DIGITS_GROUP));
            }
            if (numberGroup == string.Empty) {
                return string.Empty;
            }
            char[] digits = numberGroup.ToCharArray();
            Array.Reverse(digits);
            string lowestOrderDigitAsWordFragment = wordMapper.ConvertDigitIntoWordOfSingleDigitNumbers(digits[0], digits.Length);
            string connector = digits[0] == ConversionsConstants.CH_ZERO ? string.Empty : DASH_CONNECTOR;
            string middleOrderDigitAsWordFragment = digits.Length >= MAX_DIGITS_GROUP - 1 ? wordMapper.ConvertDigitIntoWordOfTens(digits[MAX_DIGITS_GROUP - 2]) + connector : string.Empty;
            string highestOrderDigitAsWordFragment = digits.Length == MAX_DIGITS_GROUP ? GetGroupFragment(wordMapper.ConvertDigitIntoWordOfSingleDigitNumbers(digits[MAX_DIGITS_GROUP - 1], digits.Length), ConversionsConstants.HUNDRED) : string.Empty;
            return string.Format("{0}{1}{2}", highestOrderDigitAsWordFragment, middleOrderDigitAsWordFragment, lowestOrderDigitAsWordFragment);
        }

        public string GetGroupFragment(string numberAsWords, string unit) {
            return numberAsWords == string.Empty ? string.Empty : string.Format("{0} {1} ", numberAsWords, unit);
        }
    }
}
