using System;
namespace diceRoller{
    class Dice{
        public int Roll(int numSides){
            Random myRand = new Random();
            int myRoll = myRand.Next(1,numSides+1);
            return myRoll;
        }
    }
}