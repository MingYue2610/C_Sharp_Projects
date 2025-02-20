// using System;

// namespace Program;

// public class CurrencyConverter{
//     const double gbpToEuroRate = 1.17;
//     const double euroToGbpRate = 0.85;

//     public static void Main(string[] args)
//     {
//         bool continueProgram = true;

//         while (continueProgram)
//         {
//             Console.WriteLine("\n=== Currency Converter ===");
//             Console.WriteLine("1. GBP to Euro");
//             Console.WriteLine("2. Euro to GBP");
//             Console.WriteLine("3. Exit");
//             Console.Write("Please select an option (1-3): ");

//             string choice = Console.ReadLine();

//             switch (choice)
//             {
//                 case "1":
//                     ConvertGBPToEuro();
//                     break;
//                 case "2":
//                     ConvertEuroToGBP();
//                     break;
//                 case "3":
//                     continueProgram = false;
//                     Console.WriteLine("Thank you for using the Currency Converter!");
//                     break;
//                 default:
//                     Console.WriteLine("Invalid option. Please try again.");
//                     break;
//             }
//         }
//     }

//     public static void ConvertGBPToEuro()
//     {
//         bool validInput = false;
//         double gbpAmount;
//         do
//         {
//             Console.Write("\nEnter amount in GBP: £");
//             if (double.TryParse(Console.ReadLine(), out gbpAmount))
//             {
//                 if (gbpAmount >= 0)
//                 {
//                     validInput = true;
//                     double euroAmount = gbpAmount * gbpToEuroRate;
//                     Console.WriteLine($"£{gbpAmount:F2} = €{euroAmount:F2}");
//                     DisplayExchangeRate("GBP", "Euro", gbpToEuroRate);
//                 }
//                 else
//                 {
//                     Console.WriteLine("Error: Please enter a positive amount.");
//                 }
//             }
//             else
//             {
//                 Console.WriteLine("Invalid amount. Please enter a valid number.");
//             }
//         } while (!validInput);
//     }

//     public static void ConvertEuroToGBP()
//     {
//         bool validInput = false;
//         double euroAmount;

//         do
//         {
//             Console.Write("\nEnter amount in Euro: €");
//             if (double.TryParse(Console.ReadLine(), out euroAmount))
//             {
//                 if (euroAmount >= 0)
//                 {
//                     validInput = true;
//                     double gbpAmount = euroAmount * euroToGbpRate;
//                     Console.WriteLine($"€{euroAmount:F2} = £{gbpAmount:F2}");
//                     DisplayExchangeRate("Euro", "GBP", euroToGbpRate);
//                 }
//                 else
//                 {
//                     Console.WriteLine("Error: Please enter a positive amount.");
//                 }
//             }
//             else
//             {
//                 Console.WriteLine("Invalid amount. Please enter a valid number.");
//             }
//         } while (!validInput);
//     }
    

//     public static void DisplayExchangeRate(string fromCurrency, string toCurrency, double rate)
//     {
//         Console.WriteLine($"Exchange Rate: 1 {fromCurrency} = {rate:F2} {toCurrency}");
//     }
// }


