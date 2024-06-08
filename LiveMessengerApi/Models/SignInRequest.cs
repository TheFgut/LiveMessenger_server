namespace LiveMessengerApi.Models
{
    public class SignInRequest
    {
        public string token {  get; set; }
        public string phone_number { get; set; }
        public string password { get; set; }
    }
}
