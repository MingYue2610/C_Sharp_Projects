using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculatorApp
{
    public class Calculator
    {
        public double Add(List<double> numbers)
        {
            return numbers.Sum();
        }

        public double Subtract(List<double> numbers)
        {
            return numbers.Count > 0 ? numbers[0] - numbers.Skip(1).Sum() : 0;
        }

        public double Multiply(List<double> numbers)
        {
            return numbers.Aggregate((x, y) => x * y);
        }

        public double Divide(List<double> numbers)
        {
            try
            {
                return numbers.Skip(1).Any(n => n == 0)
                    ? throw new DivideByZeroException()
                    : numbers.Skip(1).Aggregate(numbers[0], (x, y) => x / y);
            }
            catch (DivideByZeroException)
            {
                throw new DivideByZeroException("Cannot divide by zero!");
            }
        }
    }
}



