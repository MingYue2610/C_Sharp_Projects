using System;
using System.Collections.Generic;

namespace CurrencyConverterApp
{
    public class CurrencyConverter
    {
        private readonly Dictionary<string, double> conversionRates = new()
        {
            { "GBPtoUSD", 1.30 },
            { "USDtoGBP", 0.77 },
            { "EuroToGBP", 0.85 },
            { "GBPtoEuro", 1.17 },
            { "YenToGBP", 0.0051 },
            { "GBPtoYen", 197.6 }
        };

        // Currency conversion method
        public double ConvertCurrency(double amount, string fromCurrency, string toCurrency)
        {
            if (amount < 0) // Handling negative values
            {
                throw new ArgumentException("Amount cannot be negative"); 
            }

            string conversionKey = fromCurrency + "to" + toCurrency;

            if (!conversionRates.TryGetValue(conversionKey, out double rate))
            {
                throw new ArgumentException("Conversion rate for specified currency pair not found.");
            }

            return amount * rate;
        }

        // Method to retrieve the conversion rate
        public double GetConversionRate(string fromCurrency, string toCurrency)
        {
            string conversionKey = fromCurrency + "to" + toCurrency;

            if (!conversionRates.TryGetValue(conversionKey, out double rate))
            {
                throw new ArgumentException("Conversion rate for specified currency pair not found.");
            }

            return rate;
        }
    }
}
