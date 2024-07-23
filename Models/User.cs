    namespace Form_Function_App.Models
    {
    public class User
    {
        public int Id { get; set; }
        //public string Status { get; set; } = "pending";

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
    }
}