using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MvcWebApplication.Controllers;
using MvcWebApplication.ViewFunctions;
using MvcWebApplication.ViewModels.Orders;
using NUnit.Framework;
using SharedLibrary.Common.Models;
using SharedLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NUnitTestProject.WebAppTests.OrdersControllerTests
{
	public class OrderTests : Given
	{
		private IOrdersViewFunctions _iOrdersViewFunctions;
		private OrdersController _ordersController;

		[SetUp]
		public void SetUp()
		{
			_iOrdersViewFunctions = ServiceProvider.GetService<IOrdersViewFunctions>();
			_ordersController = new OrdersController(MockOrdersControllerLogger.Object, _iOrdersViewFunctions);
			_ordersController.ControllerContext = new ControllerContext
			{
				HttpContext = FakeHttpContext
			};
		}

		[TearDown]
		public void TearDown()
		{
			_iOrdersViewFunctions = null;
			_ordersController = null;
		}

		[Test]
		public async Task ControllerIndexEmptyOrderSearch()
		{
			var orderSearch = new OrderSearch()
			{
				// NOTE: OrdersViewFunction GetUserOrders will add current UserId automatically
				UserId = null,
				BeginOrderDate = null,
				EndOrderDate = null
			};

			var actionResult = await _ordersController.Index(orderSearch);

			var viewResult = actionResult as ViewResult;
			var IndexViewModel = (IndexViewModel)viewResult.Model;

			// Then the result should be
			Assert.That(IndexViewModel.OrderList.Count, Is.EqualTo(1));
			// Can also verify a specific value
			Assert.That(IndexViewModel.OrderList[0].OrderId, Is.EqualTo("12345"));
		}

		[Test]
		public async Task ControllerIndexBeginDateOrderSearch()
		{
			var orderSearch = new OrderSearch()
			{
				// NOTE: OrdersViewFunction GetUserOrders will add current UserId automatically
				UserId = null,
				BeginOrderDate = DateTime.Now.Date,
				EndOrderDate = null
			};

			var actionResult = await _ordersController.Index(orderSearch);

			var viewResult = actionResult as ViewResult;
			var IndexViewModel = (IndexViewModel)viewResult.Model;

			// Then the result should be
			Assert.That(IndexViewModel.OrderList.Count, Is.EqualTo(2));
			// Can also verify a specific value
			Assert.That(IndexViewModel.OrderList[0].OrderId, Is.EqualTo("11111"));
		}

		[Test]
		public async Task ControllerIndexBeginAndEndDateOrderSearch()
		{
			var orderSearch = new OrderSearch()
			{
				// NOTE: OrdersViewFunction GetUserOrders will add current UserId automatically
				UserId = null,
				BeginOrderDate = DateTime.Now.Date,
				EndOrderDate = DateTime.Now.Date.AddDays(10)
			};

			var actionResult = await _ordersController.Index(orderSearch);

			var viewResult = actionResult as ViewResult;
			var IndexViewModel = (IndexViewModel)viewResult.Model;

			// Then the result should be
			Assert.That(IndexViewModel.OrderList.Count, Is.EqualTo(3));
			// Can also verify a specific value
			Assert.That(IndexViewModel.OrderList[0].OrderId, Is.EqualTo("00000"));
		}
	}
}
