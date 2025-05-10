using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MvcWebApplication.Models
{
	public class Order
	{
		[Required]
		[MinLength(1)]
		[MaxLength(50)]
		[Display(Name = "Order Id")]
		public string OrderId { get; set; }

		[Required]
		[MinLength(1)]
		[MaxLength(450)]
		[Display(Name = "User Id")]
		public string UserId { get; set; }

		[Required]
		[Range(0, 9999999999999999.99)]
		[Display(Name = "Order Total")]
		public decimal OrderTotal { get; set; }

		[Required]
		[Display(Name = "Order Date")]
		public DateTime OrderDate { get; set; }


		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}

	}
}
