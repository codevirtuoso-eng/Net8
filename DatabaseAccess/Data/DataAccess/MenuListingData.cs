using DatabaseAccess.Data.Context;
using DatabaseAccess.Data.EntityModels;
using DatabaseAccess.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedLibrary.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseAccess.Data.DataAccess
{
	public class MenuListingData : IMenuListingData
	{
		private readonly MainAppDbContext _mainAppDbContext;
		private readonly ILogger<MenuListingData> _logger;

		public MenuListingData(MainAppDbContext mainAppDbContext, ILogger<MenuListingData> logger)
		{
			_mainAppDbContext = mainAppDbContext;
			_logger = logger;
			_logger.LogDebug("NLog injected into MenuListingData");
		}

		public async Task<List<MenuListingDAO>> GetMenuListings(MenuListingSearch menuListingSearch)
		{
			_logger.LogInformation($"GetMenuListings was called with menuListingSearch: {menuListingSearch}");

			var query = _mainAppDbContext.MenuListings.AsQueryable();

			if (!string.IsNullOrEmpty(menuListingSearch.Category))
			{
				query = query.Where(m => m.Category == menuListingSearch.Category);
			}

			List<MenuListingDAO> menuListingDAOList = await query.ToListAsync<MenuListingDAO>();

			return menuListingDAOList;
		}

		public async Task CreateMenuListing(MenuListingDAO menuListingDAO)
		{
			_logger.LogInformation($"CreateMenuListing was called with menuListingDAO: {menuListingDAO}");

			await _mainAppDbContext.MenuListings.AddAsync(menuListingDAO);
			await _mainAppDbContext.SaveChangesAsync();
		}

		public async Task<MenuListingDAO> GetMenuListing(int itemId)
		{
			_logger.LogInformation($"GetMenuListing was called with itemId: {itemId}");

			var query = _mainAppDbContext.MenuListings.AsQueryable();

			// first retrieve record 
			var dbMenuListingr = await query.Where(m => m.ItemId == itemId).FirstAsync();

			return dbMenuListingr;
		}

		public async Task UpdateMenuListing(MenuListingDAO menuListingDAO)
		{
			_logger.LogInformation($"UpdateMenuListing was called with menuListingDAO: {menuListingDAO}");

			var query = _mainAppDbContext.MenuListings.AsQueryable();

			// first retrieve record to be updated 
			var menuListing = await query.Where(m => m.ItemId == menuListingDAO.ItemId).FirstAsync();

			// apply the change to the dbContext so Entity Framework can track the change 
			menuListing.ItemId = menuListingDAO.ItemId;
			menuListing.Name = menuListingDAO.Name;
			menuListing.Category = menuListingDAO.Category;
			menuListing.Cost = menuListingDAO.Cost;

			// Apply updates to allow Entity Framework to track changes and create update SQL
			_mainAppDbContext.MenuListings.Update(menuListing);

			await _mainAppDbContext.SaveChangesAsync();
		}

		public async Task DeleteMenuListing(int itemId)
		{
			_logger.LogInformation($"DeleteMenuListing was called with itemId: {itemId}");

			var query = _mainAppDbContext.MenuListings.AsQueryable();

			// first retrieve record to be deleted 
			var dbMenuListing = await query.Where(m => m.ItemId == itemId).FirstAsync();

			// apply the change to the dbContext so Entity Framework can track the change 
			_mainAppDbContext.Remove(dbMenuListing);

			_mainAppDbContext.SaveChanges(); // commit the changes to the database 
		}
	}
}
