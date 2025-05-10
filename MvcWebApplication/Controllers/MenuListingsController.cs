using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcWebApplication.ViewModels.MenuListings;
using SharedLibrary.Common.Models;
using SharedLibrary.Enums;
using System.Threading.Tasks;
using System;
using MvcWebApplication.ViewFunctions;

namespace MvcWebApplication.Controllers
{
	public class MenuListingsController : Controller
	{
		private readonly ILogger<MenuListingsController> _logger;
		private readonly IMenuListingViewFunctions _menuListingViewFunctions;

		public MenuListingsController(ILogger<MenuListingsController> logger, IMenuListingViewFunctions menuListingViewFunctions)
		{
			_logger = logger;
			_menuListingViewFunctions = menuListingViewFunctions;
			_logger.LogDebug(1, "NLog injected into MenuListingsController");
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Index()
		{
			_logger.LogInformation($"Index was called");
			var indexViewModel = new IndexViewModel();

			try
			{
				await _menuListingViewFunctions.ProcessIndexRequest(indexViewModel, HttpContext);
			}
			catch (Exception ex)
			{
				// Log the exception and return a friendly message back to the client
				_logger.LogError(ex, "Error occurred searching menu listings.");
				indexViewModel.Message = ex.Message;
			}

			return View(indexViewModel);
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Search(MenuListingSearch menuListingSearch)
		{
			_logger.LogInformation($"Search was called with menuListingSearch: {menuListingSearch}");
			var searchViewModel = new SearchViewModel();
			searchViewModel.MenuListingSearch = menuListingSearch; ;

			try
			{
				await _menuListingViewFunctions.ProcessSearchRequest(searchViewModel, HttpContext);
			}
			catch (Exception ex)
			{
				// Log the exception and return a friendly message back to the client
				_logger.LogError(ex, "Error occurred searching menu listings.");
				searchViewModel.Message = ex.Message;
			}

			return View(searchViewModel);
		}
	}
}
