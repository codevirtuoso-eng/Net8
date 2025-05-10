
﻿using MvcWebApplication.Models;
using System.Collections.Generic;

namespace MvcWebApplication.ViewModels.ShoppingCarts
{
	public class IndexViewModel : BaseViewModel
	{
		public IndexViewModel()
		{
			CartItems = new List<ShoppingCartItem>();
		}

		public List<ShoppingCartItem> CartItems { get; set; }
		public decimal TotalAmount { get; set; }
	}
}
