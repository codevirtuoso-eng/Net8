using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvcWebApplication.Models;
using MvcWebApplication.ViewModels.Orders;
using SharedLibrary.DTO;
using SharedLibrary.DTO.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MvcWebApplication.ViewFunctions
{
	public class OrdersViewFunctions : IOrdersViewFunctions
	{
		private readonly ILogger<OrdersViewFunctions> _logger;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;

		public OrdersViewFunctions(ILogger<OrdersViewFunctions> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_logger = logger;
			_configuration = configuration;
			_logger.LogDebug("NLog injected into OrdersViewFunctions");
		}

		public async Task GetOrders(IndexViewModel IndexViewModel, HttpContext httpContext)
		{
			_logger.LogInformation($"GetOrders was called with IndexViewModel: {IndexViewModel}");

			// get token from the HttpContext so we can add it to the authorization header
			var token = httpContext.GetTokenAsync("access_token").Result;
			var user = httpContext.User;
			var userId = user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

			// Not good practice to pass MVC model to web API - separation of concerns
			// Convert orderSearchViewModel.OrderSearch to OrderSearchDTO
			var orderSearchDto = new OrderSearchRequestDTO()
			{
				UserId = userId,
				BeginOrderDate = IndexViewModel.OrderSearch.BeginOrderDate,
				EndOrderDate = IndexViewModel.OrderSearch.EndOrderDate
			};

			// Serialize the data to be posted
			var jsonSearch = JsonSerializer.Serialize(orderSearchDto);
			var data = new StringContent(jsonSearch, Encoding.UTF8, "application/json");

			var baseAddress = new Uri(_configuration.GetValue<string>("Misc:BaseWebApiUrl"));
			var response = String.Empty; // no ""

			// Create instance of HttpClientFacory
			var client = _httpClientFactory.CreateClient("LocalClient");
			client.BaseAddress = baseAddress;

			// Add authorization header
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			// To add a cookie to the request instead of using authorization header
			//client.DefaultRequestHeaders.Add("Cookie", $"X-Access-Token={token}");
			//client.DefaultRequestHeaders.Add("Cookie", $"X-Usernam={user.Identity.Name}");

			HttpResponseMessage httpResponse = await client.PostAsync("/api/Orders/GetOrders", data);
			httpResponse.EnsureSuccessStatusCode();
			if (httpResponse.IsSuccessStatusCode)
			{
				response = await httpResponse.Content.ReadAsStringAsync();
			}

			var results = JsonSerializer.Deserialize<List<OrderSearchResponseDTO>>(response);

			// Not good practice to pass DTO into upper layers - separation of concerns
			// Thus need to convert DTO into another class used within a view model
			foreach (var orderDto in results)
			{
				var order = new Order()
				{
					OrderId = orderDto.OrderId,
					UserId = orderDto.UserId,
					OrderDate = orderDto.OrderDate,
					OrderTotal = orderDto.OrderTotal
				};

				IndexViewModel.OrderList.Add(order);
			}

			return;
		}

		public async Task CreateOrder(string userId, HttpContext httpContext)
		{
			throw new NotImplementedException();
		}

		public async Task GetOrderDetails(string orderId, string userId, GetOrderDetailsViewModel getOrderDetailsViewModel, HttpContext httpContext)
		{
			throw new NotImplementedException();
		}


		public async Task GetUserOrders(UserOrdersViewModel userOrdersViewModel, HttpContext httpContext)
		{
			throw new NotImplementedException();
		}
	}
}
