
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedLibrary.DTO.ShoppingCart
{
	public class CartItemDTO
	{
		[JsonPropertyName("menulistingid")]
		public int MenuListingId { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("price")]
		public decimal Price { get; set; }

		[JsonPropertyName("quantity")]
		public int Quantity { get; set; }
	}

	public class ShoppingCartGetResponseDTO
	{
		[JsonPropertyName("cartid")] 
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

		[JsonPropertyName("items")]
		public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}
