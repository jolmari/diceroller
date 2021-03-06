using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DiceRollerFunctions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace DiceRollerFunctions
{
    public static class DiceCalculationFunction
    {
        [FunctionName("CalculateDicePoolRoll")]
        public static async Task<IActionResult> CalculateDicePoolRoll(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "calculate")] HttpRequest req,
            [Table("DiceRolls")] CloudTable cloudTable)
        {
            using (var reader = new StreamReader(req.Body))
            {
                var content = await reader.ReadToEndAsync();
                var payload = JsonConvert.DeserializeObject<DicePool>(content);

                if (string.IsNullOrWhiteSpace(payload.PlayerName))
                {
                    return new BadRequestObjectResult("Please type in a player name");
                }

                if (payload.PlayerName.Length > 255)
                {
                    return new BadRequestObjectResult("Choose a shorter player name");
                }

                if (!payload.Rolls.Any(r => r.Amount > 0))
                {
                    return new BadRequestObjectResult("Select at least one die to roll");
                }

                if (payload.Rolls.Sum(r => r.Amount) > 100)
                {
                    return new BadRequestObjectResult("Maximum amount (100) of dice exceeded");
                }

                var diceRollResults = payload.Rolls
                    .Where(r => r.Amount > 0)
                    .Select(r => r.Roll())
                    .OrderBy(r => r.Sides);

                // Insert roll to table storage. Add RowKey as a timestamp ticks.
                // This guarantees that the table insertion order is newest first.
                var insertOperation = TableOperation.Insert(new DiceRollRecordTableEntity
                {
                    PartitionKey = payload.PlayerName,
                    RowKey = (DateTime.MaxValue - DateTime.UtcNow).Ticks.ToString("d19"),
                    JsonDiceRollRecord = JsonConvert.SerializeObject(new DiceRollRecord
                    {
                        PlayerName = payload.PlayerName,
                        TimestampUtc = DateTime.UtcNow,
                        DiceRolls = diceRollResults
                    })
                });

                await cloudTable.ExecuteAsync(insertOperation);

                return new OkObjectResult(diceRollResults);
            }
        }

        [FunctionName("RetrieveDicePoolRoll")]
        public static async Task<IActionResult> RetrieveDicePoolRoll(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "retrieve")] HttpRequest req,
            [Table("DiceRolls")] CloudTable cloudTable)
        {
            var tableQuery = new TableQuery<DiceRollRecordTableEntity>();
            var queryResult = await cloudTable.ExecuteQuerySegmentedAsync(tableQuery, null);

            var rollRecords = queryResult
                .Where(r => !string.IsNullOrWhiteSpace(r.JsonDiceRollRecord))
                .Select(r => JsonConvert.DeserializeObject<DiceRollRecord>(r.JsonDiceRollRecord))
                .OrderByDescending(r => r.TimestampUtc);

            return new OkObjectResult(rollRecords);
        }

        [FunctionName("CleanRollHistory")]
        public static async Task<IActionResult> CleanRollHistory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "clean")] HttpRequest req,
            [Table("DiceRolls")] CloudTable cloudTable)
        {
            var tableQuery = new TableQuery<DiceRollRecordTableEntity>();
            var queryResult = await cloudTable.ExecuteQuerySegmentedAsync(tableQuery, null);

            foreach (var row in queryResult)
            {
                var deleteOperation = TableOperation.Delete(row);
                await cloudTable.ExecuteAsync(deleteOperation);
            }

            return new OkResult();
        }
    }
}
