using Microsoft.AspNetCore.Mvc.Rendering;
using MvcWebApplication.Models;
using SharedLibrary.Common.Models;
using System.Collections.Generic;

namespace MvcWebApplication.ViewModels.Shopping
{
	public class IndexViewModel : BaseViewModel
	{
		public IndexViewModel()
		{
			MenuItems = new List<MenuListing>();
		}

		public List<MenuListing> MenuItems { get; set; }
		public string Message { get; set; }
	}
}