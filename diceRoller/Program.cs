using System;
namespace diceRoller{
    public class Program{
        public static void Main(){
            Console.WriteLine("Enter number of sides on your dice");
            int myNum = int.Parse(Console.ReadLine());
            Dice myDice = new Dice();
            Console.WriteLine("Your dice roll was..." + myDice.Roll(myNum));
        }
    }
}