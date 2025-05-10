using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.DTO.Order
{
	public class OrderCreateRequestDTO
	{
		[JsonPropertyName("userid")]
		[Required]
		public string UserId { get; set; }

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}
