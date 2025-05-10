using DatabaseAccess.Data.DataAccess;
using DatabaseAccess.Data.EntityModels;
using DatabaseAccess.Data.Interfaces;
using NUnit.Framework;
using SharedLibrary.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitTestProject.DatabaseAccessTests.MenuListingDataTests
{
	public class MenuListingTests : Given
	{
		private IMenuListingData _menuListingData;

		[SetUp]
		public void SetUp()
		{
			_menuListingData = new MenuListingData(MainAppDbContext, MockMenuListingDataLogger.Object);
		}

		[TearDown]
		public void TearDown()
		{
			_menuListingData = null;
		}

		[OneTimeTearDown] // Called after all tests have completed
		public void OneTimeTearDown()
		{
			MainAppDbContext.Database.EnsureDeleted(); // necessary so next test set can create it's own DbContext with it's own data
		}

		[Test, Order(1)]
		public async Task GetMenuListingsShouldReturnAllListings()
		{
			var menuListingSearch = new MenuListingSearch()
			{
				Category = null
			};

			var results = await _menuListingData.GetMenuListings(menuListingSearch);

			// Then the result should be
			Assert.That(results.Count, Is.EqualTo(2));
		}

		[Test, Order(2)]
		public async Task CreateMenuListingShouldReturnSuccessful()
		{
			var menuListingDAO = new MenuListingDAO()
			{
				ItemId = 3,
				Category = "Side",
				Name = "French Fries",
				Cost = 3.95M
			};

			await _menuListingData.CreateMenuListing(menuListingDAO);

			var newMenuListing = MainAppDbContext.MenuListings.Where(x => x.ItemId == 3).First();

			// Then the result should be
			Assert.That(newMenuListing.Category, Is.EqualTo("Side"));
			Assert.That(newMenuListing.Cost, Is.EqualTo(3.95M));
		}

		[Test, Order(3)]
		public async Task GetMenuListingShouldReturnSingleListing()
		{
			var menuListing = await _menuListingData.GetMenuListing(1);

			// Then the result should be
			Assert.That(menuListing.Name, Is.EqualTo("Iced Tea"));
			Assert.That(menuListing.Cost, Is.EqualTo(1.50M));
		}

		[Test, Order(4)]
		public async Task UpdateMenuListingShouldReturnSuccessful()
		{
			var menuListingDAO = new MenuListingDAO()
			{
				ItemId = 3,
				Category = "Side",
				Name = "Chips and Salsa",
				Cost = 4.95M
			};

			await _menuListingData.UpdateMenuListing(menuListingDAO);

			var newMenuListing = MainAppDbContext.MenuListings.Where(x => x.ItemId == 3).First();

			// Then the result should be
			Assert.That(newMenuListing.Name, Is.EqualTo("Chips and Salsa"));
			Assert.That(newMenuListing.Cost, Is.EqualTo(4.95M));
		}

		[Test, Order(5)]
		public async Task DeleteMenuListingShouldReturnSuccessful()
		{
			await _menuListingData.DeleteMenuListing(3);

			var newMenuListings = MainAppDbContext.MenuListings.ToList();

			// Then the result should be
			Assert.That(newMenuListings.Count, Is.EqualTo(2));
		}
	}
}
