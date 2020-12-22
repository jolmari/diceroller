using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceRollerFunctions.Models
{
    public class DiceRollResult
    {
        public int Sides { get; set; }
        public int Result { get; set; }
    }

    public class DiceRoll
    {
        private readonly int _sides;
        private readonly int _amount;

        public DiceRoll(int sides, int amount)
        {
            _sides = sides;
            _amount = amount;
        }

        public IEnumerable<DiceRollResult> Roll()
        {
            var random = new Random();
            return Enumerable.Range(1, _amount)
                .Select(r => new DiceRollResult
                {
                    Sides = _sides,
                    Result = random.Next(1, _sides + 1)
                });
        }
    }
}
