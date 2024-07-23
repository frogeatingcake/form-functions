using Azure;
using Azure.Data.Tables;
using Form_Function_App.Models;
using Form_Function_App.Models.Dtos;
using Microsoft.Extensions.Logging;

namespace Form_Function_App.Services
{
    public class DataFromTableService
    {
        private readonly string _tableName = "UserTable";
        private readonly string _connectionString;
        private readonly ILogger<DataFromTableService> _logger;

        public DataFromTableService(ILogger<DataFromTableService> Logger)
        {
            _logger = Logger;
            _connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        }


        public async Task<List<UserTableDto>> RetrieveUsersAsync()
        {
            TableClient tableClient = new TableClient(_connectionString, _tableName);

            var users = new List<UserTableDto>();

            await foreach (var user in tableClient.QueryAsync<UserTableDto>())
            {
                users.Add(user);
            }

            return users;
        }

        public async Task UpdateUserAsync(UserTableDto user)
        {
            TableClient tableClient = new TableClient(_connectionString, _tableName);

            await tableClient.UpdateEntityAsync(user, ETag.All, TableUpdateMode.Replace);
        }
    }


}

