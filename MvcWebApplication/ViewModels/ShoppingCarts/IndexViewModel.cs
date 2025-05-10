
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
using MvcWebApplication.Models;
using System.Collections.Generic;

namespace MvcWebApplication.ViewModels.ShoppingCarts
{
    public class IndexViewModel
    {
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
        public decimal CartTotal { get; set; }
        public string Message { get; set; }
    }
}
