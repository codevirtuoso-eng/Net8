
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.DTO.ShoppingCart
{
	public class ShoppingCartCreateRequestDTO
	{
		[JsonPropertyName("menuitemid")]
		public int MenuItemId { get; set; }

		[JsonPropertyName("quantity")]
		public int Quantity { get; set; }

		[JsonPropertyName("userid")]
		public string UserId { get; set; }

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}
