using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcWebApplication.ViewFunctions;
using MvcWebApplication.ViewModels.Home;
using System;
using System.Threading.Tasks;

namespace MvcWebApplication.Controllers
{
    public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IHomeViewFunctions _homeViewFunctions;

		public HomeController(ILogger<HomeController> logger, IHomeViewFunctions homeViewFunctions)
		{
			_logger = logger;
			_homeViewFunctions = homeViewFunctions;
			_logger.LogDebug("NLog injected into HomeController");
		}

		public IActionResult Index()
		{
			_logger.LogInformation("Index was called");
			return View();
		}

		 [Authorize(Roles = "User, Admin")]
		public async Task<IActionResult> Privacy()
		{
			var accessToken = await HttpContext.GetTokenAsync("access_token");
			var user = HttpContext.User;  // no claims until the user has an authorized token
			_logger.LogInformation("Privacy was called");
			return View();
		}

		public IActionResult Login()
		{
			_logger.LogInformation($"Login was called.");

			return View(new HomeLoginViewModel());
		}

		public async Task<IActionResult> Authenticate(HomeLoginViewModel homeLoginViewModel)
		{
			_logger.LogInformation($"Authenticate was called homeLoginViewModel: {homeLoginViewModel}");
			try
			{
				if(!ModelState.IsValid)
				{
					// Here for demo purposes only; ErrorMessage should just used for displaying exceptions not validation
					homeLoginViewModel.Message = "UserName and UserPassword are required.";

					// If we had some custom validation, then it would go here and display custom message
					// Example: Validation which depends upon the values of different properties
					// Required values are handled automatically by data annotations
					ModelState.AddModelError(String.Empty, "Login validation failed : UserName and/or UserPassword are missing.");
				}
				else
				{
					await _homeViewFunctions.Login(homeLoginViewModel);

					if (homeLoginViewModel.UserLogin.IsAuthenticated)
					{
						// login successful, return to the home page
						return RedirectToAction("Index");
					}
				}
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error occurred during authentication.");

				homeLoginViewModel.Message = ex.Message;
			}

			return View(homeLoginViewModel); // login not successful, return to authenticate
		}

		// Commented out since we are using custom error handling
		//[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		//public IActionResult Error()
		//{
		//    _logger.LogInformation("Error was called");
		//    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		//}

	}
}