using DatabaseAccess.Data.Context;
using DatabaseAccess.Data.DataAccess;
using DatabaseAccess.Data.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NUnitTestProject.DatabaseAccessTests.OrderDataTests
{
	[TestFixture]
	public class Given
	{
		// Mock logger since dependency injection requires it; we really do not want any logging
		protected readonly Mock<ILogger<OrderData>> MockOrdersDataLogger = new Mock<ILogger<OrderData>>();

		protected readonly DbContextOptions<MainAppDbContext> MainAppDbContextOptions;
		protected readonly MainAppDbContext MainAppDbContext;

		private List<OrderDAO> OrderList; // will hold in memory data to perform data access tests
		private List<OrderDetailDAO> OrderDetailList;

		// setups the conditions needed to perform the various tests
		// since the actual database is external to the web API, we must mock the database
		// in this case we mock it using an in-memory database
		public Given() 
		{
			MainAppDbContextOptions = new DbContextOptionsBuilder<MainAppDbContext>()
				.UseInMemoryDatabase(databaseName: "InMemoryAppDb")
				.Options;

			MainAppDbContext = new MainAppDbContext(MainAppDbContextOptions);

			PopulateOrdersList();
			PopulateOrderDetailsList();
		}

		private void PopulateOrdersList()
		{
			// Create list to be added to in-memory database table
			OrderList = new List<OrderDAO>
			{
				new OrderDAO{ OrderId="12345", UserId="FakeId", OrderTotal=10.00M, OrderDate=DateTime.Now.AddDays(-3) },
				new OrderDAO{ OrderId="78900", UserId="FakeId", OrderTotal=20.00M, OrderDate=DateTime.Now.AddDays(3) }
			};

			// Add list to the DbContext
			MainAppDbContext.AddRange(OrderList);
			MainAppDbContext.SaveChanges();
		}

		private void PopulateOrderDetailsList()
		{
			// Create list to be added to in-memory database table
			OrderDetailList = new List<OrderDetailDAO>();
			OrderDetailList.Add(new OrderDetailDAO
			{
				OrderId = "12345",
				OrderDetailId = "A1",
				ItemId = 1001,
				Category = "Sandwich",
				Name = "Cheeseburger",
				Cost = 8.00M
			});
			OrderDetailList.Add(new OrderDetailDAO
			{
				OrderId = "78900",
				OrderDetailId = "A2",
				ItemId = 1002,
				Category = "Sandwich",
				Name = "Hamburger",
				Cost = 7.00M
			});

			// Add list to the DbContext
			MainAppDbContext.AddRange(OrderDetailList);
			MainAppDbContext.SaveChanges();
		}
	}
}
