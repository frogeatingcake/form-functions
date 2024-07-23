using Form_Function_App.Models;
using Form_Function_App.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Form_Function_App
{
    public class RetryHandle
    {
        private readonly RetryHandleService _retryHandleService;
        private readonly ILogger<RetryHandle> _logger;

        public RetryHandle(RetryHandleService retryHandleService, ILogger<RetryHandle> logger)
        {
            _retryHandleService = retryHandleService;
            _logger = logger;
        }

        [Function("RetryHandle")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"RetryHandler Function executed at: {DateTime.Now}");

            await _retryHandleService.ProcessPendingUsersAsync();
        }
    }
}