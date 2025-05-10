
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcWebApplication.ViewFunctions;
using MvcWebApplication.ViewModels.ShoppingCarts;
using System;
using System.Threading.Tasks;

namespace MvcWebApplication.Controllers
{
	public class ShoppingCartsController : Controller
	{
		private readonly ILogger<ShoppingCartsController> _logger;
		private readonly IShoppingCartViewFunctions _shoppingCartViewFunctions;

		public ShoppingCartsController(ILogger<ShoppingCartsController> logger, IShoppingCartViewFunctions shoppingCartViewFunctions)
		{
			_logger = logger;
			_shoppingCartViewFunctions = shoppingCartViewFunctions;
			_logger.LogDebug(1, "NLog injected into ShoppingCartsController");
		}

		[Authorize(Roles = "User")]
		public async Task<IActionResult> Index()
		{
			_logger.LogInformation($"Index was called");
			var indexViewModel = new IndexViewModel();

			try
			{
				await _shoppingCartViewFunctions.ProcessIndexRequest(indexViewModel, HttpContext);
			}
			catch (Exception ex)
			{
				// Log the exception and return a friendly message back to the client
				_logger.LogError(ex, "Error occurred loading shopping cart.");
				indexViewModel.Message = ex.Message;
			}

			return View(indexViewModel);
		}

		[Authorize(Roles = "User")]
		[HttpPost]
		public async Task<IActionResult> RemoveItem(int itemId)
		{
			_logger.LogInformation($"RemoveItem was called with itemId: {itemId}");

			try
			{
				await _shoppingCartViewFunctions.ProcessRemoveItemRequest(itemId, HttpContext);
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				// Log the exception and return a friendly message back to the client
				_logger.LogError(ex, "Error occurred removing item from cart.");
				return RedirectToAction("Index", new { message = ex.Message });
			}
		}

		[Authorize(Roles = "User")]
		[HttpPost]
		public async Task<IActionResult> EmptyCart()
		{
			_logger.LogInformation($"EmptyCart was called");

			try
			{
				await _shoppingCartViewFunctions.ProcessEmptyCartRequest(HttpContext);
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				// Log the exception and return a friendly message back to the client
				_logger.LogError(ex, "Error occurred emptying cart.");
				return RedirectToAction("Index", new { message = ex.Message });
			}
		}

		[Authorize(Roles = "User")]
		[HttpPost]
		public async Task<IActionResult> Checkout()
		{
			_logger.LogInformation($"Checkout was called");

			try
			{
				await _shoppingCartViewFunctions.ProcessCheckoutRequest(HttpContext);
				return RedirectToAction("UserOrders", "Orders");
			}
			catch (Exception ex)
			{
				// Log the exception and return a friendly message back to the client
				_logger.LogError(ex, "Error occurred during checkout.");
				return RedirectToAction("Index", new { message = ex.Message });
			}
		}
	}
}
