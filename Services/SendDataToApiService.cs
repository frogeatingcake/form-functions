using Form_Function_App.Models.Dtos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Form_Function_App.Services
{
    public class SendDataToApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SendDataToApiService> _logger;

        public SendDataToApiService(ILogger<SendDataToApiService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task SendToApiAsync(UserDto user)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://dd4e-196-229-18-119.ngrok-free.app/api/user", content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully sent data to API");
            }
            else
            {
                _logger.LogError($"Failed to send data to API: {response.StatusCode}");
                throw new Exception("Failed to send data to API");
            }
        }

        public async Task<bool> IsApiAvailableAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7085/api/user");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking API availability: {ex.Message}");
                return false;
            }
        }
    }
}
