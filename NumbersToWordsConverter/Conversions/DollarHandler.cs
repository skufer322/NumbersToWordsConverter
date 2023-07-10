namespace Conversions {

    /// <summary>
    /// Abstract class implementing most of the methods for converting a number-based representation of a currency of units and possibly subunits into a word-based representation.
    /// For a concrete currency, an instantiable subclasses must be implemented which determines the names of the units and subunits.
    /// </summary>
    internal abstract class ACurrencyHandler {

        static readonly string EXC_MSG_MAX_NUMBER_EXCEEDED_TF = "The given number of {0} ('{1}') exceeds the allowed maximum number of {0} ('{2}').";

        private readonly INumberAsGroupsOf3Handler numberAsGroupsHandler;

        public ACurrencyHandler(INumberAsGroupsOf3Handler numberAsGroupsHandler) {
            this.numberAsGroupsHandler = numberAsGroupsHandler;
        }

        /// <summary>
        /// Converts the currency's number-based representation of units and possibly subunits into its word-based representation.
        /// </summary>
        /// <param name="units">number-based representation of the currency's units to be converted into its word-based representation</param>
        /// <param name="subunits">optional number-based representation of the currency's subunits to be converted into its word-based representation (pass null if there are not subunits)</param>
        /// <returns>word-based representation of the currency's units and possibly subunits</returns>
        /// <exception cref="ArgumentException">if the number of units is greater than 999,999,999 and the number of subunits is greater than 99</exception>
        public string ConvertCurrencyToWords(string units, string? subunits) {
            if (units.Length > ConversionsConstants.MAX_DIGITS_UNIT) {
                throw new ArgumentException(string.Format(EXC_MSG_MAX_NUMBER_EXCEEDED_TF, GetUnitsName(), units, new string(ConversionsConstants.CH_9, ConversionsConstants.MAX_DIGITS_UNIT)));
            }
            // account for units
            string words = ConvertNumberIntoWords(units);
            words = AddCurrencyNameWithCorrectCardinality(words, GetUnitName(), GetUnitsName());
            if (subunits != null) {
                // also account for subunits
                if (subunits.Length > ConversionsConstants.MAX_DIGITS_SUBUNIT) {
                    throw new ArgumentException(string.Format(EXC_MSG_MAX_NUMBER_EXCEEDED_TF, GetSubunitsName(), subunits, new string(ConversionsConstants.CH_9, ConversionsConstants.MAX_DIGITS_SUBUNIT)));
                }
                string subunitsAsWords = ConvertNumberIntoWords(subunits);
                subunitsAsWords = AddCurrencyNameWithCorrectCardinality(subunitsAsWords, GetSubunitName(), GetSubunitsName());
                words = string.Format("{0} and {1}", words, subunitsAsWords);
            }
            return words;
        }

        private string ConvertNumberIntoWords(string number) {
            string hundredsGroup = numberAsGroupsHandler.GetHundredsGroup(number);
            string thousandsGroup = numberAsGroupsHandler.GetThousandsGroup(number);
            string millionsGroup = numberAsGroupsHandler.GetMillionsGroup(number);

            string hgAsWords = numberAsGroupsHandler.ConvertNumberGroupIntoWords(hundredsGroup);
            string tgAsWords = numberAsGroupsHandler.ConvertNumberGroupIntoWords(thousandsGroup);
            string mgAsWords = numberAsGroupsHandler.ConvertNumberGroupIntoWords(millionsGroup);

            return string.Format("{0}{1}{2}", numberAsGroupsHandler.GetGroupFragment(mgAsWords, ConversionsConstants.MILLION, true), numberAsGroupsHandler.GetGroupFragment(tgAsWords, ConversionsConstants.THOUSAND, true), hgAsWords);
        }

        private static string AddCurrencyNameWithCorrectCardinality(string numberAsWords, string currencyNameSingular, string currencyNamePlural) {
            return string.Format("{0} {1}", numberAsWords, numberAsWords == ConversionsConstants.W_ONE ? currencyNameSingular : currencyNamePlural);
        }

        protected abstract string GetUnitName();

        protected abstract string GetUnitsName();

        protected abstract string GetSubunitName();

        protected abstract string GetSubunitsName();
    }

    /// <summary>
    /// Instantiable class of <see cref="ACurrencyHandler"/> for dollars.
    /// </summary>
    internal class DollarHandler : ACurrencyHandler {

        public DollarHandler(INumberAsGroupsOf3Handler numberAsGroupsOf3Handler) : base(numberAsGroupsOf3Handler) {
        }

        protected override string GetUnitName() {
            return ConversionsConstants.DOLLAR;
        }

        protected override string GetUnitsName() {
            return ConversionsConstants.DOLLARS;
        }

        protected override string GetSubunitName() {
            return ConversionsConstants.CENT;
        }

        protected override string GetSubunitsName() {
            return ConversionsConstants.CENTS;
        }
    }
}
