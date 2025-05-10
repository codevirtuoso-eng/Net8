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
			_logger.LogInformation($"CreateOrder was called with userId: {userId}");

			// get token from the HttpContext so we can add it to the authorization header
			var token = await httpContext.GetTokenAsync("access_token");

			// Create the request DTO
			var orderCreateRequestDTO = new OrderCreateRequestDTO()
			{
				UserId = userId
			};

			// Serialize the data to be posted
			var jsonRequest = JsonSerializer.Serialize(orderCreateRequestDTO);
			var data = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

			var baseAddress = new Uri(_configuration.GetValue<string>("Misc:BaseWebApiUrl"));

			// Create instance of HttpClientFacory
			var client = _httpClientFactory.CreateClient("LocalClient");
			client.BaseAddress = baseAddress;

			// Add authorization header
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			HttpResponseMessage httpResponse = await client.PostAsync("/api/Orders/CreateOrder", data);
			httpResponse.EnsureSuccessStatusCode();

			// We don't need to do anything with the response for this method
		}

		public async Task GetOrderDetails(string orderId, string userId, GetOrderDetailsViewModel getOrderDetailsViewModel, HttpContext httpContext)
		{
			_logger.LogInformation($"GetOrderDetails was called with orderId: {orderId}, userId: {userId}");

			// get token from the HttpContext so we can add it to the authorization header
			var token = await httpContext.GetTokenAsync("access_token");

			// Create the request DTO
			var orderGetRequestDTO = new OrderGetRequestDTO()
			{
				UserId = userId,
				OrderId = orderId
			};

			// Serialize the data to be posted
			var jsonRequest = JsonSerializer.Serialize(orderGetRequestDTO);
			var data = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

			var baseAddress = new Uri(_configuration.GetValue<string>("Misc:BaseWebApiUrl"));
			var response = String.Empty;

			// Create instance of HttpClientFacory
			var client = _httpClientFactory.CreateClient("LocalClient");
			client.BaseAddress = baseAddress;

			// Add authorization header
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			HttpResponseMessage httpResponse = await client.PostAsync("/api/Orders/GetOrderDetails", data);
			httpResponse.EnsureSuccessStatusCode();

			if (httpResponse.IsSuccessStatusCode)
			{
				response = await httpResponse.Content.ReadAsStringAsync();

				// Deserialize the response
				var orderDetailsDto = JsonSerializer.Deserialize<OrderGetResponseDTO>(response);

				// Map the response to the view model
				getOrderDetailsViewModel.OrderId = orderDetailsDto.OrderId;
				getOrderDetailsViewModel.UserId = orderDetailsDto.UserId;
				getOrderDetailsViewModel.OrderDate = orderDetailsDto.OrderDate;
				getOrderDetailsViewModel.OrderTotal = orderDetailsDto.OrderTotal;

				// Map the order details
				foreach (var detailDto in orderDetailsDto.OrderDetails)
				{
					getOrderDetailsViewModel.OrderDetails.Add(new OrderDetail
					{
						OrderId = orderId,
						ItemId = detailDto.ItemId.ToString(),
						Name = detailDto.Name,
						Cost = detailDto.Cost,
						Quantity = detailDto.Quantity,
						LineTotal = detailDto.LineTotal
					});
				}
			}
		}


		public async Task GetUserOrders(UserOrdersViewModel userOrdersViewModel, HttpContext httpContext)
		{
			_logger.LogInformation($"GetUserOrders was called with UserOrdersViewModel: {userOrdersViewModel}");

			// get token from the HttpContext so we can add it to the authorization header
			var token = await httpContext.GetTokenAsync("access_token");
			var user = httpContext.User;

			// Ensure the UserId is set in the search criteria
			if (string.IsNullOrEmpty(userOrdersViewModel.OrderSearch.UserId))
			{
				// Try to get the admin user ID if available (for admin users viewing all orders)
				var isAdmin = user.IsInRole("Admin");
				if (isAdmin)
				{
					// For admin users, we'll leave the UserId null to get all orders
				}
				else
				{
					// For non-admin users, we'll get their own orders
					userOrdersViewModel.OrderSearch.UserId = user.Claims.First(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
				}
			}

			// Convert orderSearchViewModel.OrderSearch to OrderSearchDTO
			var orderSearchDto = new OrderSearchRequestDTO()
			{
				UserId = userOrdersViewModel.OrderSearch.UserId,
				BeginOrderDate = userOrdersViewModel.OrderSearch.BeginOrderDate,
				EndOrderDate = userOrdersViewModel.OrderSearch.EndOrderDate
			};

			// Serialize the data to be posted
			var jsonSearch = JsonSerializer.Serialize(orderSearchDto);
			var data = new StringContent(jsonSearch, Encoding.UTF8, "application/json");

			var baseAddress = new Uri(_configuration.GetValue<string>("Misc:BaseWebApiUrl"));
			var response = String.Empty;

			// Create instance of HttpClientFacory
			var client = _httpClientFactory.CreateClient("LocalClient");
			client.BaseAddress = baseAddress;

			// Add authorization header
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			HttpResponseMessage httpResponse = await client.PostAsync("/api/Orders/GetOrders", data);
			httpResponse.EnsureSuccessStatusCode();
			if (httpResponse.IsSuccessStatusCode)
			{
				response = await httpResponse.Content.ReadAsStringAsync();
			}

			var results = JsonSerializer.Deserialize<List<OrderSearchResponseDTO>>(response);

			// Convert DTOs to models for the view
			foreach (var orderDto in results)
			{
				var order = new Order()
				{
					OrderId = orderDto.OrderId,
					UserId = orderDto.UserId,
					OrderDate = orderDto.OrderDate,
					OrderTotal = orderDto.OrderTotal
				};

				userOrdersViewModel.OrderList.Add(order);
			}

			// If user is admin, let's get the list of users for the dropdown
			var isUserAdmin = user.IsInRole("Admin");
			if (isUserAdmin)
			{
				// In a real application, you would call an API to get user list
				// For now, we'll add a placeholder user
				userOrdersViewModel.UserList.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
				{
					Value = "",
					Text = "-- All Users --"
				});

				// Add current user
				var currentUserId = user.Claims.First(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
				var currentUserName = user.Identity.Name;
				userOrdersViewModel.UserList.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
				{
					Value = currentUserId,
					Text = currentUserName
				});
			}
		}

		public async Task<OrdersGetOrderDetailsViewModel> GetOrderDetailsViewModel(string orderId, string userId, HttpContext httpContext)
		{
			_logger.LogInformation($"Getting order details for orderId: {orderId}, userId: {userId}");
			var viewModel = new OrdersGetOrderDetailsViewModel();

			// get token from the HttpContext so we can add it to the authorization header
			var token = await httpContext.GetTokenAsync("access_token");

			// Create the request DTO
			var orderGetRequestDTO = new OrderGetRequestDTO()
			{
				UserId = userId,
				OrderId = orderId
			};

			// Serialize the data to be posted
			var jsonRequest = JsonSerializer.Serialize(orderGetRequestDTO);
			var data = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

			var baseAddress = new Uri(_configuration.GetValue<string>("Misc:BaseWebApiUrl"));
			var response = String.Empty;

			// Create instance of HttpClientFacory
			var client = _httpClientFactory.CreateClient("LocalClient");
			client.BaseAddress = baseAddress;

			// Add authorization header
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			HttpResponseMessage httpResponse = await client.PostAsync("/api/Orders/GetOrderDetails", data);
			httpResponse.EnsureSuccessStatusCode();

			if (httpResponse.IsSuccessStatusCode)
			{
				response = await httpResponse.Content.ReadAsStringAsync();

				// Deserialize the response
				var orderDetailsDto = JsonSerializer.Deserialize<OrderGetResponseDTO>(response);

				// Map the response to the view model
				viewModel.OrderId = orderDetailsDto.OrderId;
				viewModel.UserId = orderDetailsDto.UserId;
				viewModel.OrderDate = orderDetailsDto.OrderDate;
				viewModel.OrderTotal = orderDetailsDto.OrderTotal;

				// Map the order details
				foreach (var detailDto in orderDetailsDto.OrderDetails)
				{
					viewModel.OrderDetails.Add(new OrderDetail
					{
						OrderId = orderId,
						OrderDetailId = detailDto.OrderDetailId,
						ItemId = detailDto.ItemId.ToString(),
						Name = detailDto.Name,
						Cost = detailDto.Cost,
						Quantity = detailDto.Quantity,
						LineTotal = detailDto.LineTotal
					});
				}
			}

			return viewModel;
		}
	}
}