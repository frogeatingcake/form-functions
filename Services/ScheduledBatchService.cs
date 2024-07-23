using Azure;
using Azure.Data.Tables;
using Form_Function_App.Models;
using Form_Function_App.Models.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Form_Function_App.Services
{
    public class ScheduledBatchService
    {
        private readonly string _tableName = "UserTable";
        private readonly string _connectionString;
        private readonly ILogger<ScheduledBatchService> _logger;

        public ScheduledBatchService(ILogger<ScheduledBatchService> logger)
        {
            _logger = logger;
            _connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        }

        public async Task ProcessFailedUsersAsync()
        {
            TableClient tableClient = new TableClient(_connectionString, _tableName);

            await foreach (UserTableDto user in tableClient.QueryAsync<UserTableDto>())
            {
                if (user.Status.Equals("failed", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogInformation($"Updating user {user.RowKey} status from 'failed' to 'pending'");

                    
                    user.Status = "pending";
                    user.RetryCounter = 0; 

                    await UpdateUserAsync(user);
                }
            }
        }

        public async Task UpdateUserAsync(UserTableDto user)
        {
            TableClient tableClient = new TableClient(_connectionString, _tableName);

            await tableClient.UpdateEntityAsync(user, ETag.All, TableUpdateMode.Replace);
        }
    }
}