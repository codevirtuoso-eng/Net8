
using MvcWebApplication.Models;
using MvcWebApplication.ViewModels;
using System.Collections.Generic;

namespace MvcWebApplication.ViewModels.Shopping
{
	public class IndexViewModel : BaseViewModel
	{
		public IndexViewModel()
		{
			MenuListings = new List<MenuListing>();
		}

		public string Message { get; set; }
		public List<MenuListing> MenuListings { get; set; }
		public List<MenuListing> MenuItems { get; set; }
	}
}
