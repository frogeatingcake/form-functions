using Form_Function_App.Models;
using Form_Function_App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Form_Function_App
{
    public class SubmitData
    {
        private readonly ILogger<SubmitData> _logger;
        private readonly SubmitDataService _submitDataService;

        public SubmitData(ILogger<SubmitData> logger, SubmitDataService submitDataService)
        {
            _logger = logger;
            _submitDataService = submitDataService;
        }

        [Function("SubmitData")]
        public async Task <IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,  "post")] HttpRequest req)
        {
            _logger.LogInformation("SubmitDataFunction is Executed");

            string reqBody = await new StreamReader(req.Body).ReadToEndAsync();
            User user = JsonConvert.DeserializeObject<User>(reqBody);

            if (user == null)
                return new BadRequestObjectResult("Invalid Data");

            await _submitDataService.AddDataToQueue(user);

            return new OkObjectResult("Data has been successfully submitted and queued");
        }
    }
}
