using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedLibrary.DTO.Order
{
	public class OrderGetResponseDTO
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
		public List<OrderDetailGetResponseDTO> OrderDetails { get; set; } = new List<OrderDetailGetResponseDTO>();

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}
