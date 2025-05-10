
﻿using System.ComponentModel.DataAnnotations;

namespace MvcWebApplication.Models
{
	public class ShoppingCartItem
	{
		[Required]
		[Display(Name = "Item Id")]
		public int ItemId { get; set; }

		[Required]
		[MinLength(1)]
		[MaxLength(25)]
		[Display(Name = "Name")]
		public string Name { get; set; }

		[Required]
		[Range(1, 100)]
		[Display(Name = "Quantity")]
		public int Quantity { get; set; }

		[Required]
		[Range(0, 9999999999999999.99)]
		[Display(Name = "Unit Price")]
		public decimal UnitPrice { get; set; }

		[Required]
		[Range(0, 9999999999999999.99)]
		[Display(Name = "Total Price")]
		public decimal TotalPrice { get; set; }
	}
}
