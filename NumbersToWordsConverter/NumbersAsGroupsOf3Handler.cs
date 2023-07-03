using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversions {
    internal class NumbersAsGroupsOf3Handler {

        static readonly int MAX_DIGITS_GROUP = 3;
        static readonly string DASH_CONNECTOR = "-";

        public static string GetHundredsGroup(string number) {
            return GetCharactersOfTrailingGroup(number);
        }

        public static string GetThousandsGroup(string number) {
            int numberOfCharactersToRemoveFromEnd = MAX_DIGITS_GROUP;
            // remove last 3 characters such that the thousands are the final group of number
            return number.Length > numberOfCharactersToRemoveFromEnd ? GetCharactersOfTrailingGroup(number[0..^numberOfCharactersToRemoveFromEnd]) : string.Empty;
        }

        public static string GetMillionsGroups(string number) {
            int numberOfCharactersToRemoveFromEnd = MAX_DIGITS_GROUP * 2;
            // remove last 6 characters such that the millions are the final group of number 
            return number.Length > numberOfCharactersToRemoveFromEnd ? GetCharactersOfTrailingGroup(number[0..^numberOfCharactersToRemoveFromEnd]) : string.Empty;
        }

        public static string GetCharactersOfTrailingGroup(string number) {
            int numberOfDigits = number.Length;
            return number.Substring(Math.Max(0, numberOfDigits - MAX_DIGITS_GROUP), Math.Min(numberOfDigits, MAX_DIGITS_GROUP));
        }

        public static string ConvertNumberGroupIntoWord(string numberGroup) {
            // TODO: prüfen, ob numberGroup.Length maximal 3 ist?
            if (numberGroup == string.Empty) {
                return string.Empty;
            }
            char[] digits = numberGroup.ToCharArray();
            Array.Reverse(digits);
            string lowestOrderDigitAsWordFragment = DigitsToWordsMapper.ConvertDigitIntoWord0to9(digits[0], digits.Length);
            string connector = digits[0] == ConversionsConstants.CH_ZERO ? string.Empty : DASH_CONNECTOR;
            string middleOrderDigitAsWordFragment = digits.Length >= MAX_DIGITS_GROUP - 1 ? DigitsToWordsMapper.ConvertDigitIntoWordOfTens(digits[MAX_DIGITS_GROUP - 2]) + connector : string.Empty;
            string highestOrderDigitAsWordFragment = digits.Length == MAX_DIGITS_GROUP ? DigitsToWordsMapper.ConvertDigitIntoWord0to9(digits[MAX_DIGITS_GROUP - 1], digits.Length) + " hundred " : string.Empty;
            return string.Format("{0}{1}{2}", highestOrderDigitAsWordFragment, middleOrderDigitAsWordFragment, lowestOrderDigitAsWordFragment);
        }
    }
}
