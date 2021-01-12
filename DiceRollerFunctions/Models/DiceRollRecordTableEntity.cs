using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace DiceRollerFunctions.Models
{
    public class DiceRollRecordTableEntity : TableEntity
    {
        public string JsonDiceRollRecord { get; set; }
    }
}
