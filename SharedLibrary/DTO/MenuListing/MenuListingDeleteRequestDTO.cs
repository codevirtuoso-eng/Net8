using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.DTO.MenuListing
{
	public class MenuListingDeleteRequestDTO
	{
		[JsonPropertyName("itemid")]
		public int ItemId { get; set; }

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}
