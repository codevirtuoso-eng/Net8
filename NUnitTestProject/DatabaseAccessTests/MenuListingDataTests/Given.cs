using DatabaseAccess.Data.Context;
using DatabaseAccess.Data.DataAccess;
using DatabaseAccess.Data.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitTestProject.DatabaseAccessTests.MenuListingDataTests
{
	[TestFixture]
	public class Given
	{
		// Mock logger since dependency injection requires it; we really do not want any logging
		protected readonly Mock<ILogger<MenuListingData>> MockMenuListingDataLogger = new Mock<ILogger<MenuListingData>>();

		protected readonly DbContextOptions<MainAppDbContext> MainAppDbContextOptions;
		protected readonly MainAppDbContext MainAppDbContext;

		private List<MenuListingDAO> MenuListingList; // will hold in memory data to perform data access tests

		// setups the conditions needed to perform the various tests
		// since the actual database is external to the web API, we must mock the database
		// in this case we mock it using an in-memory database
		public Given()
		{
			MainAppDbContextOptions = new DbContextOptionsBuilder<MainAppDbContext>()
				.UseInMemoryDatabase(databaseName: "InMemoryAppDb")
				.Options;

			MainAppDbContext = new MainAppDbContext(MainAppDbContextOptions);

			PopulateMenuListingList();
		}

		private void PopulateMenuListingList()
		{
			// Create list to be added to in-memory database table
			MenuListingList = new List<MenuListingDAO>
			{
				new MenuListingDAO{ ItemId=1, Category="Beverage", Name="Iced Tea", Cost=1.50M },
				new MenuListingDAO{ ItemId=2, Category="Sandwich", Name="Cheeseburger", Cost=10.95M }
			};

			// Add list to the DbContext
			MainAppDbContext.AddRange(MenuListingList);
			MainAppDbContext.SaveChanges();
		}
	}
}
