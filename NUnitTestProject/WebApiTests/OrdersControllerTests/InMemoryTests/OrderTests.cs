using DatabaseAccess.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SharedLibrary.DTO;
using SharedLibrary.DTO.Order;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppAPI.ApiFunctions;
using WebAppAPI.Apis;

namespace UnitTestProject.WebApiTests.OrdersControllerTests.InMemoryTests
{
	public class OrderTests : Given
	{
		private IOrderData _iOrderData;
		private IOrdersFunctions _iOrdersFunctions;
		private OrdersController _ordersController;

		[SetUp]
		public void SetUp()
		{
			_iOrderData = ServiceProvider.GetService<IOrderData>();
			_iOrdersFunctions = ServiceProvider.GetService<IOrdersFunctions>();
			_ordersController = new OrdersController(_iOrdersFunctions, MockOrdersControllerLogger.Object);
			_ordersController.ControllerContext = new ControllerContext
			{
				HttpContext = FakeHttpContext
			};
		}

		[TearDown]
		public void TearDown()
		{
			_iOrderData = null;
			_iOrdersFunctions = null;
			_ordersController = null;
		}

		[OneTimeTearDown] // Called after all tests have completed
		public void OneTimeTearDown()
		{
			MainAppDbContext.Database.EnsureDeleted(); // necessary so next test set can create it's own DbContext with it's own data
		}

		[Test]
		public async Task ControllerShouldReturnAllOrdersWithEmptyOrderSearchDTO()
		{
			var orderSearchDTO = new OrderSearchRequestDTO();

			var actionResult = await _ordersController.GetOrders(orderSearchDTO);

            // Then the result should be
            ClassicAssert.IsInstanceOf<OkObjectResult>(actionResult); // this is the form using older NUnit
            Assert.That(actionResult, Is.InstanceOf<OkObjectResult>()); // this is the new form using the latest NUnit

            var objectResult = actionResult as OkObjectResult;
			var orderList = (List<OrderSearchResponseDTO>)objectResult.Value;

			Assert.That(orderList.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task ControllerShouldReturnSingleOrder()
		{
			// ToDo in class
		}

		[Test]
		public async Task OrderQueryShouldReturnNoOrders()
		{
			// ToDo in class
		}

		[Test]
		public async Task CreateOrderShouldCreateSingleOrder()
		{
			// ToDo in class
			// Actually not true since the code for CreateOrder is not available in the initial setup
		}

		[Test]
		public async Task GetOrderShouldReturnSingleOrder()
		{
			// ToDo in class
		}
	}
}
