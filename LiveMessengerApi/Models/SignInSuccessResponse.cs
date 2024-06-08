namespace LiveMessengerApi.Models
{
    public class SignInSuccessResponse
    {
        public string token { get; set; }
        public User? user {  get; set; }
    }
}
