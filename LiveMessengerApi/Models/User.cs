namespace LiveMessengerApi.Models
{
    public class User
    {
        public Guid user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone_number { get; set; }
        public string password { get; set; }
    }
}
