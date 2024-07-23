using Azure.Storage.Queues;
using Form_Function_App.Models;
using Newtonsoft.Json;
using System.Text;


namespace Form_Function_App.Services
{
    public class SubmitDataService
    {

        private readonly string _connectionString;
        private readonly string _queueName = "queue10";

        public SubmitDataService()
        {
            _connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        }

        public async Task AddDataToQueue(User user)
        {
            QueueClient queueClient = new QueueClient(_connectionString, _queueName);
            await queueClient.CreateIfNotExistsAsync();


            if (await queueClient.ExistsAsync())
            {
                string message = JsonConvert.SerializeObject(user);
                string base64Message = Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
                await queueClient.SendMessageAsync(base64Message);
            }
           

        }


    }
}
