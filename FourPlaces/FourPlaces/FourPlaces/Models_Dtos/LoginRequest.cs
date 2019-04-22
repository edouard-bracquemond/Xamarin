using Newtonsoft.Json;

namespace FourPlaces.Models
{
    public class LoginRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("password")]
        public string Password { get; set; }

        public LoginRequest(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }
    }
    
}