using System;
using Newtonsoft.Json;


namespace FourPlaces.Models
{
	public class CommentItem
	{
		[JsonProperty("date")]
		public DateTime Date { get; set; }
		
		[JsonProperty("author")]
		public UserItem Author { get; set; }
		
		[JsonProperty("text")]
		public string Text { get; set; }

        public string ImageURL { get; set; }
	}
}