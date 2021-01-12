using System.Collections.Generic;

namespace DiceRollerFunctions.Models
{
    public class DicePool
    {
        public string PlayerName { get; set; }
        public List<DiceRoll> Rolls { get; set; }
    }
}
