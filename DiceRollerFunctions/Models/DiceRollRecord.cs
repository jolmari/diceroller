using System;
using System.Collections.Generic;

namespace DiceRollerFunctions.Models
{
    public class DiceRollRecord
    {
        public string PlayerName { get; set; }
        public DateTime TimestampUtc { get; set; }
        public DateTime TimestampLocal => TimestampUtc.ToLocalTime();
        public IEnumerable<DiceRoll> DiceRolls { get; set; }
    }
}
