using Microsoft.AspNetCore.Mvc.Rendering;
using MvcWebApplication.Models;
using SharedLibrary.Common.Models;
using System.Collections.Generic;

namespace MvcWebApplication.ViewModels.MenuListings
{
	public class IndexViewModel : BaseViewModel
	{
		public IndexViewModel()
		{
			MenuListingSearch = new MenuListingSearch();
		}

		public List<MenuListing> MenuListingList { get; set; } = new List<MenuListing>();

		public List<SelectListItem> CategoryList { get; set; } = new List<SelectListItem>();

		public MenuListingSearch MenuListingSearch { get; set; }
	}
}
