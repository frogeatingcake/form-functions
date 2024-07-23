using AutoMapper;
using Form_Function_App.Models;
using Form_Function_App.Models.Dtos;
using Form_Function_App.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Form_Function_App.Services
{
    public class RetryHandleService
    {
        private readonly DataFromTableService _userTableService;
        private readonly SendDataToApiService _sendDataToApiService;
        private readonly ILogger<RetryHandleService> _logger;
        private readonly SendEmailService _emailService;
        private readonly IMapper _mapper;

        public RetryHandleService(SendEmailService emailService, DataFromTableService userTableService, SendDataToApiService sendDataToApiService, ILogger<RetryHandleService> logger, IMapper mapper)
        {
            _userTableService = userTableService;
            _sendDataToApiService = sendDataToApiService;
            _emailService = emailService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task ProcessPendingUsersAsync()
        {
            var users = await _userTableService.RetrieveUsersAsync();

            if (!users.Any())
            {
                _logger.LogInformation("No pending users found in table storage. Exiting function.");
                return;
            }

            foreach (var userTable in users.Where(u => u.Status == "pending"))
            {
                try
                {
                    if (userTable.RetryCounter == 2)
                    {
                        userTable.Status = "Failed";
                        _logger.LogInformation($"User {userTable.RowKey} marked as failed after 3 retries.");
                        var emailBody = $"User {userTable.RowKey} has failed after 3 retries.";
                        await _emailService.SendEmailAsync("Rafik.Zoubli@hypotheker.nl", "User Data Failed", emailBody);
                    }
                    else
                    {
                        if (await _sendDataToApiService.IsApiAvailableAsync())
                        {
                            var userDto = _mapper.Map<UserDto>(userTable); // Map UserTableDto to UserDto

                            // Optionally map UserDto to User if needed
                            var user = _mapper.Map<User>(userDto);

                            await _sendDataToApiService.SendToApiAsync(userDto);
                            userTable.Status = "Successful";
                            _logger.LogInformation($"User {userTable.RowKey} data sent successfully.");
                        }
                        else
                        {
                            userTable.RetryCounter++;
                            _logger.LogInformation($"User {userTable.RowKey} retry counter increased to {userTable.RetryCounter}.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error sending data for user {userTable.RowKey}: {ex.Message}");
                    userTable.RetryCounter++;
                }

                await _userTableService.UpdateUserAsync(userTable);
            }
        }
    }
}
