using Newtonsoft.Json;

namespace TD.Api.Dtos
{
	public class RegisterRequest
	{
		[JsonProperty("email")]
		public string Email { get; set; }
		
		[JsonProperty("first_name")]
		public string FirstName { get; set; }
		
		[JsonProperty("last_name")]
		public string LastName { get; set; }
		
		[JsonProperty("password")]
		public string Password { get; set; }

        public RegisterRequest(string email,string password, string lastname, string firstname)
        {
            this.Email = email;
            this.Password = password;
            this.LastName = lastname;
            this.FirstName = firstname;
        }
	}

}