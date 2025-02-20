using System;
using System.Collections.Generic;

namespace CalculatorApp
{
    public class CalculatorUI
    {
        private readonly Calculator _calculator;

        public CalculatorUI()
        {
            _calculator = new Calculator();
        }

        public void DisplayMenu()
        {
            Console.WriteLine("\nSelect operation:");
            Console.WriteLine("1. Add");
            Console.WriteLine("2. Subtract");
            Console.WriteLine("3. Multiply");
            Console.WriteLine("4. Divide");
            Console.WriteLine("5. Exit");
            Console.Write("Enter choice (1-5): ");
        }

        public int GetValidChoice()
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 5)
                {
                    return choice;
                }
                Console.WriteLine("Invalid choice! Please enter a number between 1 and 5.");
                DisplayMenu();
            }
        }

        public List<double> GetNumbers()
        {
            int count;
            
            // Loop until a valid count is entered
            while (true)
            {
                Console.Write("How many numbers do you want to calculate with? ");
                if (int.TryParse(Console.ReadLine(), out count) && count >= 2)
                {
                    break;  // Exit the loop when valid input is provided
                }
                Console.WriteLine("Please enter a valid number (minimum 2)!");
            }

            List<double> numbers = new List<double>();

            for (int i = 0; i < count; i++)
            {
                double number;
                
                // Loop until a valid number is entered for each item in the list
                while (true)
                {
                    Console.Write($"Enter number {i + 1}: ");
                    if (double.TryParse(Console.ReadLine(), out number))
                    {
                        numbers.Add(number);
                        break;  // Exit the loop when valid input is provided
                    }
                    Console.WriteLine("Invalid number entered! Please enter a valid number.");
                }
            }

            return numbers;
        }

        public double Calculate(int choice, List<double> numbers)
        {
            return choice switch
            {
                1 => _calculator.Add(numbers),
                2 => _calculator.Subtract(numbers),
                3 => _calculator.Multiply(numbers),
                4 => _calculator.Divide(numbers),
                _ => throw new ArgumentException("Invalid operation")
            };
        }

        public string GetOperationSymbol(int choice)
        {
            return choice switch
            {
                1 => "+",
                2 => "-",
                3 => "*",
                4 => "/",
                _ => ""
            };
        }

        public void DisplayResult(List<double> numbers, string operation, double result)
        {
            Console.WriteLine($"\nResult: {string.Join($" {operation} ", numbers)} = {result}");
        }

        public bool ContinueCalculating()
        {
            Console.Write("\nDo you want to perform another calculation? (y/n): ");
            return Console.ReadLine()?.ToLower().StartsWith("y") ?? false;
        }
    }
}