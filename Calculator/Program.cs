using System;

namespace CalculatorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var ui = new CalculatorUI();
            bool continueCalculating = true;

            while (continueCalculating)
            {
                try
                {
                    ui.DisplayMenu();
                    int choice = ui.GetValidChoice();

                    if (choice == 5)
                    {
                        Console.WriteLine("Thank you for using the calculator!");
                        break;
                    }

                    var numbers = ui.GetNumbers();
                    string operation = ui.GetOperationSymbol(choice);
                    double result = ui.Calculate(choice, numbers);
                    ui.DisplayResult(numbers, operation, result);
                    
                    continueCalculating = ui.ContinueCalculating();
                }
                catch (DivideByZeroException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    continueCalculating = ui.ContinueCalculating();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    continueCalculating = ui.ContinueCalculating();
                }
            }
        }
    }
}