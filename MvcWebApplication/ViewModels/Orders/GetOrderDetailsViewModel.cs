
﻿using MvcWebApplication.Models;
using System;
using System.Collections.Generic;

namespace MvcWebApplication.ViewModels.Orders
{
	public class GetOrderDetailsViewModel : BaseViewModel
	{
		public GetOrderDetailsViewModel()
		{
			OrderDetails = new List<OrderDetail>();
		}

		public string OrderId { get; set; }
		public string UserId { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal OrderTotal { get; set; }
		public List<OrderDetail> OrderDetails { get; set; }

		// holds value to determine which order search to return (Index, GetOrders)
		public string SearchSource { get; set; }
	}
}
