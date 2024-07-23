using System;
using Form_Function_App.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Form_Function_App
{
    public class ScheduledBatch
    {
        private readonly ILogger _logger;
        private readonly ScheduledBatchService _scheduledBatchService;

        public ScheduledBatch(ILoggerFactory loggerFactory, ScheduledBatchService scheduledBatchService)
        {
            _logger = loggerFactory.CreateLogger<ScheduledBatch>();
            _scheduledBatchService = scheduledBatchService;
        }

        [Function("ScheduledBatch")]
        public async Task Run([TimerTrigger("0 */4 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            await _scheduledBatchService.ProcessFailedUsersAsync();


        }
    }
}
