using Form_Function_App.Models;
using Form_Function_App.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;


namespace Form_Function_App
{
    public class QueueDataProcessing
    {
        private readonly ILogger<QueueDataProcessing> _logger;
        //private readonly SendDataToApiService _sendDataToApiService;
        private readonly QueueToTableService _queueToTableService;

        public QueueDataProcessing(ILogger<QueueDataProcessing> logger, SendDataToApiService sendDataToApiService, QueueToTableService queueToTableService)
        {
            _logger = logger;
            //_sendDataToApiService = sendDataToApiService;
            _queueToTableService = queueToTableService;
        }

        [Function("QueueDataProcessing")]
        public async Task Run([QueueTrigger("queue10", Connection = "AzureWebJobsStorage")] string queueItem)
        {
            _logger.LogInformation("Queue trigger function processed");

            try
            {
                var user = JsonSerializer.Deserialize<User>(queueItem);
                if (user != null)
                {

                    await _queueToTableService.SaveDataToTable(user);


                }
                else
                {
                    _logger.LogWarning("Failed to deserialize the queue message");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred while processing queue item: {ex.Message}");
            }
        }
    }
}
