using AutoMapper;
using Azure.Data.Tables;
using Form_Function_App.Models;
using Form_Function_App.Models.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Form_Function_App.Services
{
    public class QueueToTableService
    {
        private readonly string _connectionString;
        private readonly string _tableName = "UserTable";
        private readonly ILogger<QueueToTableService> _logger;
        private readonly IMapper _mapper;
        private readonly TableClient _tableClient;

        public QueueToTableService(ILogger<QueueToTableService> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            _tableClient = new TableClient(_connectionString, _tableName);
        }

        public async Task SaveDataToTable(User user)
        {
            try
            {
                await _tableClient.CreateIfNotExistsAsync();
                var userDto = _mapper.Map<UserTableDto>(user);
                await _tableClient.AddEntityAsync(userDto);
                _logger.LogInformation("Data stored in Table Storage successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error storing data in Table Storage: {ex.Message}");
                throw;
            }
        }
    }
}
