using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DiceRollerFunctions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DiceRollerFunctions
{
    public static class DiceCalculationFunction
    {
        [FunctionName("DiceCalculationFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "calculate")] HttpRequest req)
        {
            using (var reader = new StreamReader(req.Body))
            {
                var content = await reader.ReadToEndAsync();
                var payload = JsonConvert.DeserializeObject<DicePool>(content);

                var result = payload.Rolls
                    .SelectMany(r => r.Roll())
                    .OrderBy(r => r.Sides);

                return new OkObjectResult(result);
            }
        }
    }
}
