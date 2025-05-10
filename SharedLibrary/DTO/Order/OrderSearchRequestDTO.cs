using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedLibrary.DTO.Order
{
	public class OrderSearchRequestDTO
	{
		[JsonPropertyName("beginorderdate")]
		public DateTime? BeginOrderDate { get; set; }

		[JsonPropertyName("endorderdate")]
		public DateTime? EndOrderDate { get; set; }

		[JsonPropertyName("userid")]
		public string UserId { get; set; }

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}
