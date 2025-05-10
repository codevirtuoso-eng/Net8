using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SharedLibrary.DTO.Order
{
	public class OrderGetResponseDTO
	{
		[Required]
		[JsonPropertyName("orderId")]
		public string OrderId { get; set; }

		[Required]
		[JsonPropertyName("userId")]
		public string UserId { get; set; }

		[Required]
		[JsonPropertyName("orderDate")]
		public DateTime OrderDate { get; set; }

		[Required]
		[JsonPropertyName("orderTotal")]
		public decimal OrderTotal { get; set; }

		[Required]
		[JsonPropertyName("orderDetails")]
		public List<OrderDetailGetResponseDTO> OrderDetails { get; set; }

		public OrderGetResponseDTO()
		{
			OrderDetails = new List<OrderDetailGetResponseDTO>();
		}
	}
}