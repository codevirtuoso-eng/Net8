using DatabaseAccess.Data.DataAccess;
using DatabaseAccess.Data.EntityModels;
using DatabaseAccess.Data.Interfaces;
using NUnit.Framework;
using SharedLibrary.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUnitTestProject.DatabaseAccessTests.OrderDataTests
{
	public class OrderTests : Given
	{
		private IOrderData _orderData;

		[SetUp]
		public void SetUp()
		{
			_orderData = new OrderData(MainAppDbContext, MockOrdersDataLogger.Object);
		}

		[TearDown]
		public void TearDown()
		{
			_orderData = null;
		}

		[OneTimeTearDown] // Called after all tests have completed
		public void OneTimeTearDown()
		{
			MainAppDbContext.Database.EnsureDeleted(); // necessary so next test set can create it's own DbContext with it's own data
		}

		[Test]
		public async Task OrderQueryShouldReturnSingleOrder()
		{
			var orderSearch = new OrderSearch()
			{
				BeginOrderDate = DateTime.Now
			};

			var results = await _orderData.GetOrders(orderSearch);

			// Then the result should be
			Assert.That(results.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task OrderQueryShouldReturnNoOrders()
		{
			var orderSearch = new OrderSearch()
			{
				BeginOrderDate = DateTime.Now.AddDays(10)
			};

			var results = await _orderData.GetOrders(orderSearch);

			// Then the result should be
			Assert.That(results.Count, Is.EqualTo(0));
		}

		[Test]
		public async Task GetOrderShouldReturnSingleOrder()
		{
			// ToDo in class
		}

		[Test]
		public async Task GetOrderDetailsShouldReturnSingleOrderDetail()
		{
			// ToDo in class
		}

		[Test]
		public async Task CreateOrderShouldReturnSuccessful()
		{
			// ToDo in class
		}
	}
}
