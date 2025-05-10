using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcWebApplication.ViewFunctions;
using MvcWebApplication.ViewModels.Orders;
using SharedLibrary.Common.Models;
using SharedLibrary.Enums;
using System;
using System.Threading.Tasks;

namespace MvcWebApplication.Controllers
{

	public class OrdersController : Controller
	{
		private readonly ILogger<OrdersController> _logger;
		private readonly IOrdersViewFunctions _ordersViewFunctions;

		public OrdersController(ILogger<OrdersController> logger, IOrdersViewFunctions ordersViewFunctions)
		{
			_logger = logger;
			_ordersViewFunctions = ordersViewFunctions;
			_logger.LogDebug(1, "NLog injected into OrdersController");
		}

		[Authorize(Roles = "User")]
		public async Task<IActionResult> Index(OrderSearch orderSearch)
		{
			_logger.LogInformation($"Index was called with orderSearch: {orderSearch}");
			var IndexViewModel = new IndexViewModel();
			IndexViewModel.OrderSearch = orderSearch;

			try
			{
				await _ordersViewFunctions.GetOrders(IndexViewModel, HttpContext);
			}
			catch (Exception ex)
			{
				// Log the exception and return a friendly message back to the client
				_logger.LogError(ex, "Error occurred searching orders.");
				IndexViewModel.Message = ex.Message;
			}

			return View(IndexViewModel);
		}

		[Authorize(Roles = "User")]
		public async Task<IActionResult> CreateOrder(string userId)
		{
			_logger.LogInformation($"CreateOrder was called with userId: {userId}");


			try
			{
				// code goes here
				return View();
			}
			catch (Exception ex)
			{
				// Log the exception and return a friendly message back to the client
				_logger.LogError(ex, "Error occurred creating order from the shopping cart.");
				var indexViewModel = new IndexViewModel();
				indexViewModel.Message = ex.Message;
				return View("Index", indexViewModel);
			}
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UserOrders(OrderSearch orderSearch)
		{
			_logger.LogInformation($"UserOrders was called with orderSearch: {orderSearch}");
			var userOrdersViewModel = new UserOrdersViewModel();
			userOrdersViewModel.OrderSearch = orderSearch;

			try
			{
				// code goes here
			}
			catch (Exception ex)
			{
				// Log the exception and return a friendly message back to the client
				_logger.LogError(ex, "Error occurred searching user orders.");
				userOrdersViewModel.Message = ex.Message;
			}

			return View(userOrdersViewModel);
		}

		[Authorize(Roles = "User, Admin")]
		public async Task<IActionResult> GetOrderDetails(string orderId, string userId, string searchSource)
		{
			_logger.LogInformation($"GetOrderDetails was called with orderId: {orderId} userId: {userId} searchSource: {searchSource}");
			var getOrderDetailsViewModel = new GetOrderDetailsViewModel();
			getOrderDetailsViewModel.SearchSource = searchSource;

			try
			{
				// need code here
			}
			catch (Exception ex)
			{
				// Log the exception and return a friendly message back to the client
				_logger.LogError(ex, "Error occurred getting order details.");
				getOrderDetailsViewModel.Message = ex.Message;
			}

			return View(getOrderDetailsViewModel);
		}
	}
}
