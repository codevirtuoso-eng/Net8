using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedLibrary.DTO.ShoppingCart
{
	public class ShoppingCartGetResponseDTO
	{
		[JsonPropertyName("cartid")] // helps with de-serialization
		public int CartId { get; set; }

		[JsonPropertyName("userid")]
		public string UserId { get; set; }

		[JsonPropertyName("itemid")]
		public int ItemId { get; set; }

		[JsonPropertyName("category")]
		public string Category { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("cost")]
		public decimal Cost { get; set; }

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}
