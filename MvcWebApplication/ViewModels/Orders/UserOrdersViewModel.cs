using Microsoft.AspNetCore.Mvc.Rendering;
using MvcWebApplication.Models;
using SharedLibrary.Common.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace MvcWebApplication.ViewModels.Orders
{
	public class UserOrdersViewModel : BaseViewModel
	{		public UserOrdersViewModel()
		{
			OrderList = new List<Order>(); // advisable to ensure list is always initialized
			OrderSearch = new OrderSearch();
			UserList = new List<SelectListItem>();
		}

		public List<Order> OrderList { get; set; }

		public List<SelectListItem> UserList { get; set; }

		public OrderSearch OrderSearch { get; set; }

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}

