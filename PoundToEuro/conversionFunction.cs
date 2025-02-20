using System;

namespace CurrencyConverterApp
{
    public class CurrencyConverter
    {
        const double gbpToEuroRate = 1.17; // Rate for conversion, can be amended or obtained via API call in future
        const double EuroToGbpRate = 0.85;

        public double ConvertGBPToEuro(double gbpAmount)
        {
            if (gbpAmount < 0) // Check for negative value and return an error message to the user
            {
                throw new ArgumentException("Amount cannot be negative");
            }
            return gbpAmount * gbpToEuroRate; // Calculation to get the new rate
        }

        public double ConvertEuroToGBP(double euroAmount)
        {
            if (euroAmount < 0) // Check for negative value and return an error message to the user
            {
                throw new ArgumentException("Amount cannot be negative");
            }
            return euroAmount * EuroToGbpRate; // Calculation to get the new rate
        }

        public double GetGBPToEuroRate() // This function collects and provides the rate of the currency to be display when called upon as an object
        {
            return gbpToEuroRate;
        }

        public double GetEuroToGBPRate() // This function collects and provides the rate of the currency to be display when called upon as an object
        {
            return EuroToGbpRate;
        }
    }
}