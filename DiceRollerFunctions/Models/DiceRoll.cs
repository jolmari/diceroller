using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DiceRollerFunctions.Models
{
    public class DiceRoll
    {
        public int Sides { get; }
        public int Amount { get; }
        public IEnumerable<int> Results { get; private set; }

        public DiceRoll(int sides, int amount)
        {
            Sides = sides;
            Amount = amount;
            Results = new List<int>();
        }

        public DiceRoll Roll()
        {
            var random = new Random();
            Results =  Enumerable.Range(1, Amount)
                .Select(r => random.Next(1, Sides + 1));
            return this;
        }
    }
}
