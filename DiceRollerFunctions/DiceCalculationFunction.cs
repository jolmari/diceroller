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

                var result = payload.Rolls
                    .SelectMany(r => r.Roll())
                    .OrderBy(r => r.Sides);

                if (!result.Any())
                {
                    return new OkResult();
                }

                // Insert roll to table storage. Add RowKey as a timestamp decremented from
                // max value. This guarantees that the table insertion order is newest first.
                var insertOperation = TableOperation.Insert(new DiceRollRecord
                {
                    PartitionKey = "<UserName>",
                    RowKey = (DateTime.MaxValue - DateTime.UtcNow).Ticks.ToString("d19"),
                    JsonDiceRollRecord = JsonConvert.SerializeObject(result)
                });

                await cloudTable.ExecuteAsync(insertOperation);

                return new OkObjectResult(result);
            }
        }

        [FunctionName("RetrieveDicePoolRoll")]
        public static async Task<IActionResult> RetrieveDicePoolRoll(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "retrieve")] HttpRequest req,
            [Table("DiceRolls")] CloudTable cloudTable)
        {
            var tableQuery = new TableQuery<DiceRollRecord>();
            var queryResult = await cloudTable.ExecuteQuerySegmentedAsync(tableQuery, null);

            return new OkObjectResult(queryResult
                .Where(r => !string.IsNullOrWhiteSpace(r.JsonDiceRollRecord))
                .Select(r => JsonConvert.DeserializeObject<List<DiceRollResult>>(r.JsonDiceRollRecord)));
        }

        [FunctionName("CleanRollHistory")]
        public static async Task<IActionResult> CleanRollHistory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "clean")] HttpRequest req,
            [Table("DiceRolls")] CloudTable cloudTable)
        {
            var tableQuery = new TableQuery<DiceRollRecord>();
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
