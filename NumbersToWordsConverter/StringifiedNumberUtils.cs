namespace Conversions {
    internal class StringifiedNumberUtils {

        public static string TrimLeadingZeros(string number) {
            return number.TrimStart(ConversionsConstants.CH_ZERO);
        }

        public static string ReplaceEmptyStringWithZero(string number) {
            return number == string.Empty ? char.ToString(ConversionsConstants.CH_ZERO) : number;
        }
    }
}
