using Newtonsoft.Json;

namespace FourPlaces.Models
{
	public class ImageItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }
	}
}