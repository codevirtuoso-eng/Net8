using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SharedLibrary.DTO;
using SharedLibrary.DTO.Order;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WebAppAPI.ApiFunctions;
using WebAppAPI.Apis;

namespace NUnitTestProject.WebApiTests.OrdersControllerTests.MockedOrderDataTests
{
	public class OrderTests : Given
	{
		private IOrdersFunctions _iOrdersFunctions;
		private OrdersController _ordersController;

		[SetUp]
		public void SetUp()
		{
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
			_iOrdersFunctions = null;
			_ordersController = null;
		}

		[Test]
		public async Task ControllerShouldReturnAllOrdersWithEmptyOrderSearchDTO()
		{
			var orderSearchDTO = new OrderSearchRequestDTO()
			{
				BeginOrderDate = null,
				EndOrderDate = null
			};

			var actionResult = await _ordersController.GetOrders(orderSearchDTO);

            ClassicAssert.IsInstanceOf<OkObjectResult>(actionResult); // this is the form using older NUnit
            Assert.That(actionResult, Is.InstanceOf<OkObjectResult>()); // this is the new form using the latest NUnit

            var objectResult = actionResult as OkObjectResult;
			var orderList = (List<OrderSearchResponseDTO>)objectResult.Value;

			// Then the result should be
			Assert.That(objectResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
			Assert.That(orderList.Count, Is.EqualTo(2));
		}

		[Test]
		public async Task ControllerShouldReturnSingleOrder()
		{
			var orderSearchDTO = new OrderSearchRequestDTO()
			{
				BeginOrderDate = DateTime.Now
			};

			var actionResult = await _ordersController.GetOrders(orderSearchDTO);

            ClassicAssert.IsInstanceOf<OkObjectResult>(actionResult); // this is the form using older NUnit
            Assert.That(actionResult, Is.InstanceOf<OkObjectResult>()); // this is the new form using the latest NUnit

            var objectResult = actionResult as OkObjectResult;
			var orderList = (List<OrderSearchResponseDTO>)objectResult.Value;

			// Then the result should be
			Assert.That(objectResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
			Assert.That(orderList.Count, Is.EqualTo(1));

			// Can also verify a specific value
			Assert.That(orderList[0].OrderId, Is.EqualTo("78900"));
		}

		[Test]
		public async Task OrderQueryShouldReturnNoOrders()
		{
			// ToDo in class
		}

		[Test]
		public async Task CreateOrderShouldReturnSuccessful()
		{
			// ToDo in class
		}

		[Test]
		public async Task GetOrderShouldReturnSingleOrder()
		{
			// ToDo in class
		}
	}
}
