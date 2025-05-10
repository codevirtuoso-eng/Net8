
﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvcWebApplication.Models;
using MvcWebApplication.ViewModels.ShoppingCarts;
using SharedLibrary.DTO.Order;
using SharedLibrary.DTO.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MvcWebApplication.ViewFunctions
{
    public class ShoppingCartViewFunctions : IShoppingCartViewFunctions
    {
        private readonly ILogger<ShoppingCartViewFunctions> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ShoppingCartViewFunctions(ILogger<ShoppingCartViewFunctions> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
            _logger.LogDebug("NLog injected into ShoppingCartViewFunctions");
        }

        public async Task ProcessIndexRequest(IndexViewModel indexViewModel, HttpContext httpContext)
        {
            _logger.LogInformation($"ProcessIndexRequest was called with indexViewModel: {indexViewModel}");

            // get token from the HttpContext so we can add it to the authorization header
            var token = await httpContext.GetTokenAsync("access_token");
            var username = httpContext.User.Identity.Name;

            // Create search request
            var searchRequest = new ShoppingCartSearchRequestDTO
            {
                Username = username
            };

            // Serialize the data to be posted
            var jsonSearch = JsonSerializer.Serialize(searchRequest);
            var data = new StringContent(jsonSearch, Encoding.UTF8, "application/json");

            var baseAddress = new Uri(_configuration.GetValue<string>("Misc:BaseWebApiUrl"));
            var response = String.Empty;

            // Create instance of HttpClientFactory
            var client = _httpClientFactory.CreateClient("LocalClient");
            client.BaseAddress = baseAddress;

            // Add authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage httpResponse = await client.PostAsync("/api/ShoppingCarts/GetCart", data);
            httpResponse.EnsureSuccessStatusCode();
            if (httpResponse.IsSuccessStatusCode)
            {
                response = await httpResponse.Content.ReadAsStringAsync();
            }

            var results = JsonSerializer.Deserialize<List<ShoppingCartGetResponseDTO>>(response);

            decimal total = 0;
            foreach (var item in results)
            {
                indexViewModel.CartItems.Add(new ShoppingCartItem
                {
                    ItemId = item.ItemId,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.UnitPrice * item.Quantity
                });
                total += item.UnitPrice * item.Quantity;
            }
            indexViewModel.TotalAmount = total;
        }

        public async Task ProcessRemoveItemRequest(int itemId, HttpContext httpContext)
        {
            _logger.LogInformation($"ProcessRemoveItemRequest was called with itemId: {itemId}");

            // get token from the HttpContext so we can add it to the authorization header
            var token = await httpContext.GetTokenAsync("access_token");
            var username = httpContext.User.Identity.Name;

            var removeRequest = new ShoppingCartRemoveRequestDTO
            {
                ItemId = itemId,
                Username = username
            };

            // Serialize the data to be posted
            var jsonData = JsonSerializer.Serialize(removeRequest);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var baseAddress = new Uri(_configuration.GetValue<string>("Misc:BaseWebApiUrl"));

            // Create instance of HttpClientFactory
            var client = _httpClientFactory.CreateClient("LocalClient");
            client.BaseAddress = baseAddress;

            // Add authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage httpResponse = await client.PostAsync("/api/ShoppingCarts/RemoveItem", content);
            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task ProcessEmptyCartRequest(HttpContext httpContext)
        {
            _logger.LogInformation($"ProcessEmptyCartRequest was called");

            // get token from the HttpContext so we can add it to the authorization header
            var token = await httpContext.GetTokenAsync("access_token");
            var username = httpContext.User.Identity.Name;

            var emptyRequest = new ShoppingCartEmptyRequestDTO
            {
                Username = username
            };

            // Serialize the data to be posted
            var jsonData = JsonSerializer.Serialize(emptyRequest);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var baseAddress = new Uri(_configuration.GetValue<string>("Misc:BaseWebApiUrl"));

            // Create instance of HttpClientFactory
            var client = _httpClientFactory.CreateClient("LocalClient");
            client.BaseAddress = baseAddress;

            // Add authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage httpResponse = await client.PostAsync("/api/ShoppingCarts/EmptyCart", content);
            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task ProcessCheckoutRequest(HttpContext httpContext)
        {
            _logger.LogInformation($"ProcessCheckoutRequest was called");

            // get token from the HttpContext so we can add it to the authorization header
            var token = await httpContext.GetTokenAsync("access_token");
            var username = httpContext.User.Identity.Name;

            var orderRequest = new OrderCreateRequestDTO
            {
                Username = username
            };

            // Serialize the data to be posted
            var jsonData = JsonSerializer.Serialize(orderRequest);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var baseAddress = new Uri(_configuration.GetValue<string>("Misc:BaseWebApiUrl"));

            // Create instance of HttpClientFactory
            var client = _httpClientFactory.CreateClient("LocalClient");
            client.BaseAddress = baseAddress;

            // Add authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage httpResponse = await client.PostAsync("/api/Orders/CreateOrder", content);
            httpResponse.EnsureSuccessStatusCode();
        }
    }
}
