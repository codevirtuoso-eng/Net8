using DatabaseAccess.Data.EntityModels;
using SharedLibrary.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Data.Interfaces
{
	public interface IMenuListingData
	{
		public Task<List<MenuListingDAO>> GetMenuListings(MenuListingSearch menuListingSearch);
		public Task CreateMenuListing(MenuListingDAO menuListingDAO);
		public Task<MenuListingDAO> GetMenuListing(int itemId);
		public Task UpdateMenuListing(MenuListingDAO menuListingDAO);
		public Task DeleteMenuListing(int itemId);
	}
}
