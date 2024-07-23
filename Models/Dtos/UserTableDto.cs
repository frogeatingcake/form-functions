using Azure;
using Azure.Data.Tables;
using System;

namespace Form_Function_App.Models.Dtos
{
    public class UserTableDto : ITableEntity
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Birthday { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string HomeAddress { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Branch { get; set; }
        public string AppointmentReason { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; } = "pending";
        public int RetryCounter { get; set; } = 0;
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public UserTableDto()
        {
            PartitionKey = "test";
            RowKey = Guid.NewGuid().ToString();
        }
    }
}