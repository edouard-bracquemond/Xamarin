using Newtonsoft.Json;

namespace FourPlaces.Models
{
    public class UpdateProfileRequest
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        
        [JsonProperty("image_id")]
        public int? ImageId { get; set; }

        public UpdateProfileRequest(string firstname, string lastname, int? imageId)
        {
            this.FirstName = firstname;
            this.LastName = lastname;
            this.ImageId = imageId;
        }
    }
}