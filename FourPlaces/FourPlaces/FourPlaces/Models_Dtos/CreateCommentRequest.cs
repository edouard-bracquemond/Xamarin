using Newtonsoft.Json;

namespace FourPlaces.Models
{
	public class CreateCommentRequest
	{
        public CreateCommentRequest(string text)
        {
            Text = text;
        }

        [JsonProperty("text")]
		public string Text { get; set; }
	}
}