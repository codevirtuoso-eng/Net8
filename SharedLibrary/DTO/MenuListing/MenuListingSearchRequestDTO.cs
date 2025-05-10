using SharedLibrary.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.DTO.MenuListing
{
	public class MenuListingSearchRequestDTO
	{
		[JsonPropertyName("category")]
		public string Category { get; set; }

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}
