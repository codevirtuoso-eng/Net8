using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedLibrary.DTO.Order
{
	public class OrderSearchResponseDTO
	{
		[JsonPropertyName("orderId")] // helps with de-serialization
		public string OrderId { get; set; }

		[JsonPropertyName("userid")]
		public string UserId { get; set; }

		[JsonPropertyName("ordertotal")]
		public decimal OrderTotal { get; set; }

		[JsonPropertyName("orderdate")]
		public DateTime OrderDate { get; set; }

		[JsonPropertyName("orderdetails")]
		public List<OrderSearchDetailResponseDTO> OrderDetails { get; set; } = new List<OrderSearchDetailResponseDTO>();

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}
