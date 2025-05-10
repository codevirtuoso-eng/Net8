using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedLibrary.DTO.Order;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebAppAPI.ApiFunctions;

namespace WebAppAPI.Apis
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		private IOrdersFunctions _ordersFunction;
		private readonly ILogger<OrdersController> _logger;

		public OrdersController(IOrdersFunctions ordersFunction, ILogger<OrdersController> logger)
		{
			_ordersFunction = ordersFunction;
			_logger = logger;
			_logger.LogDebug("NLog injected into OrdersController");
		}

		[Authorize(Roles = "User, Admin")] // prefer to use enums, but requires custom attribute
		[HttpPost]
		[Route("GetOrders")]
		public async Task<ActionResult> GetOrders(OrderSearchRequestDTO orderSearchRequestDTO)
		{
			_logger.LogInformation($"GetOrders was called with orderSearchRequestDTO: {orderSearchRequestDTO}");
			var user = HttpContext.User;
			var token = HttpContext.GetTokenAsync("access_token").Result;
			var cookies = HttpContext.Request.Cookies;

			try
			{
				var result = await _ordersFunction.GetOrders(orderSearchRequestDTO);
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred getting orders.");
				var responseObject = new { responseText = ex.Message }; // Not necessarily a friendly message
				return StatusCode(StatusCodes.Status500InternalServerError, responseObject);
			}
		}

		// only User roles can create orders
		[Authorize(Roles = "User")] // prefer to use enums, but requires custom attribute
		[HttpPost]
		[Route("CreateOrder")]
		public async Task<ActionResult> CreateOrder(OrderCreateRequestDTO orderCreateRequestDTO)
		{
			return Ok();
		}

		[Authorize(Roles = "User, Admin")] // prefer to use enums, but requires custom attribute
		[HttpPost]
		[Route("GetOrder")]
		public async Task<ActionResult> GetOrderDetails(OrderGetRequestDTO orderGetRequestDTO)
		{
			return Ok();
		}

		[Authorize(Roles = "User, Admin")] // prefer to use enums, but requires custom attribute
		[HttpGet]
		[Route("SecuredTestAlive")]
		public ActionResult SecuredTestAlive()
		{
			_logger.LogInformation("Get SecuredTestAlive was called.");

			var accessToken = HttpContext.GetTokenAsync("access_token");
			var user = HttpContext.User;  // no claims until the user has an authorized token

			return Ok("Secured live service test successful.");
		}

		[AllowAnonymous]
		[HttpGet]
		[Route("FreeTestAlive")]
		public ActionResult FreeTestAlive()
		{
			_logger.LogInformation("Get FreeTestAlive was called.");

			return Ok("Free live service test successful.");
		}
	}
}
