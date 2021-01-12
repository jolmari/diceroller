using System;
using System.Collections.Generic;

namespace DiceRollerFunctions.Models
{
    public class DiceRollRecord
    {
        public string PlayerName { get; set; }
        public DateTime TimestampUtc { get; set; }
        public string TimestampFormatted => TimestampUtc.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK");
        public IEnumerable<DiceRoll> DiceRolls { get; set; }
    }
}
