using System;

namespace CurrencyConverterApp
{
    public class CurrencyConverterUI
    {
        public readonly CurrencyConverter _converter;

        public CurrencyConverterUI()
        {
            _converter = new CurrencyConverter(); // New object is created
        }

        public void RunConverter()
        {
            bool continueProgram = true; // While loop is true, it will continue to run until "3" is selected to break the loop
            while (continueProgram)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Convert("GBP","otherCurrencies");
                        break;
                    case "2":
                        Convert("otherCurrencies", "GBP");
                        break;
                    case "3":
                        continueProgram = false;
                        Console.WriteLine("Thank you for using the Currency Converter!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        public void DisplayMenu()
        {
            Console.WriteLine("\n=== Currency Converter ===");
            Console.WriteLine("1. GBP to Euro");
            Console.WriteLine("2. Euro to GBP");
            Console.WriteLine("3. Exit");
            Console.Write("Please select an option (1-3): ");
        }

        public void GBPToEuroConversion()
        {
            Console.Write("\nEnter amount in GBP: £");
            if (double.TryParse(Console.ReadLine(), out double gbpAmount))
            {
                try
                {
                    double euroAmount = _converter.ConvertGBPToEuro(gbpAmount);
                    Console.WriteLine($"£{gbpAmount:F2} = €{euroAmount:F2}");
                    DisplayExchangeRate("GBP", "Euro", _converter.GetGBPToEuroRate());
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid amount. Please enter a valid number.");
            }
        }

        public void EuroToGBPConversion()
        {
            Console.Write("\nEnter amount in Euro: €");
            if (double.TryParse(Console.ReadLine(), out double euroAmount))
            {
                try
                {
                    double gbpAmount = _converter.ConvertEuroToGBP(euroAmount);
                    Console.WriteLine($"€{euroAmount:F2} = £{gbpAmount:F2}");
                    DisplayExchangeRate("Euro", "GBP", _converter.GetEuroToGBPRate());
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid amount. Please enter a valid number.");
            }
        }

        public void DisplayExchangeRate(string fromCurrency, string toCurrency, double rate)
        {
            Console.WriteLine($"Exchange Rate: 1 {fromCurrency} = {rate:F2} {toCurrency}");
        }
    }
}