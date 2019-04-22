using Newtonsoft.Json;

namespace FourPlaces.Models
{
    public class UpdatePasswordRequest
    {
        public UpdatePasswordRequest(string oldPassword, string newPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        [JsonProperty("old_password")]
        public string OldPassword { get; set; }
        
        [JsonProperty("new_password")]
        public string NewPassword { get; set; }
    }
}