namespace Intex.Models
{
    //This model is to organize the data that is going to be fed into our prediction model
    public class PredictionInput
    {
        public int CustomerID { get; set; }
        public int Time { get; set; }
        public int Amount { get; set; }
        public int Age { get; set; }
        public int DayOfWeekMon { get; set; }
        public int DayOfWeekSat { get; set; }
        public int DayOfWeekSun { get; set; }
        public int DayOfWeekThu { get; set; }
        public int DayOfWeekTue { get; set; }
        public int DayOfWeekWed { get; set; }
        public int EntryModePIN { get; set; }
        public int EntryModeTap { get; set; }
        public int TypeOfTransactionOnline { get; set; }
        public int TypeOfTransactionPOS { get; set; }
        public int CountryOfTransactionIndia { get; set; }
        public int CountryOfTransactionRussia { get; set; }
        public int CountryOfTransactionUSA { get; set; }
        public int CountryOfTransactionUnitedKingdom { get; set; }
        public int ShippingAddressIndia { get; set; }
        public int ShippingAddressRussia { get; set; }
        public int ShippingAddressUSA { get; set; }
        public int ShippingAddressUnitedKingdom { get; set; }
        public int BankHSBC { get; set; }
        public int BankHalifax { get; set; }
        public int BankLloyds { get; set; }
        public int BankMetro { get; set; }
        public int BankMonzo { get; set; }
        public int BankRBS { get; set; }
        public int TypeOfCardVisa { get; set; }
        public int GenderM { get; set; }
        public int CountryOfResidenceIndia { get; set; }
        public int CountryOfResidenceRussia { get; set; }
        public int CountryOfResidenceUSA { get; set; }
        public int CountryOfResidenceUnitedKingdom { get; set; }
    }
}
