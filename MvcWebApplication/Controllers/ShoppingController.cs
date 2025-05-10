
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcWebApplication.ViewFunctions;
using MvcWebApplication.ViewModels.Shopping;
using SharedLibrary.Enums;
using System;
using System.Threading.Tasks;

namespace MvcWebApplication.Controllers
{
	public class ShoppingController : Controller
	{
		private readonly ILogger<ShoppingController> _logger;
		private readonly IShoppingViewFunctions _shoppingViewFunctions;

		public ShoppingController(ILogger<ShoppingController> logger, IShoppingViewFunctions shoppingViewFunctions)
		{
			_logger = logger;
			_shoppingViewFunctions = shoppingViewFunctions;
			_logger.LogDebug(1, "NLog injected into ShoppingController");
		}

		[Authorize(Roles = "User")]
		public async Task<IActionResult> Index()
		{
			_logger.LogInformation($"Index was called");
			var indexViewModel = new IndexViewModel();

			try
			{
				await _shoppingViewFunctions.ProcessIndexRequest(indexViewModel, HttpContext);
			}
			catch (Exception ex)
			{
				// Log the exception and return a friendly message back to the client
				_logger.LogError(ex, "Error occurred loading menu items for shopping.");
				indexViewModel.Message = ex.Message;
			}

			return View(indexViewModel);
		}

		[Authorize(Roles = "User")]
		[HttpPost]
		public async Task<IActionResult> AddToCart(int itemId, int quantity)
		{
			_logger.LogInformation($"AddToCart was called with itemId: {itemId}, quantity: {quantity}");

			try
			{
				await _shoppingViewFunctions.ProcessAddToCartRequest(itemId, quantity, HttpContext);
				return RedirectToAction("Index", "ShoppingCarts");
			}
			catch (Exception ex)
			{
				// Log the exception and return a friendly message back to the client
				_logger.LogError(ex, "Error occurred adding item to cart.");
				return RedirectToAction("Index", new { message = ex.Message });
			}
		}
	}
}
