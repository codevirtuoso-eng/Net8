
﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvcWebApplication.ViewModels.Shopping;
using SharedLibrary.DTO.MenuListing;
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
    public class ShoppingViewFunctions : IShoppingViewFunctions
    {
        private readonly ILogger<ShoppingViewFunctions> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ShoppingViewFunctions(ILogger<ShoppingViewFunctions> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
            _logger.LogDebug("NLog injected into ShoppingViewFunctions");
        }

        public async Task ProcessIndexRequest(IndexViewModel indexViewModel, HttpContext httpContext)
        {
            _logger.LogInformation($"ProcessIndexRequest was called with indexViewModel: {indexViewModel}");

            // get token from the HttpContext so we can add it to the authorization header
            var token = await httpContext.GetTokenAsync("access_token");
            var username = httpContext.User.Identity.Name;

            // Create search DTO with no filters to get all menu items
            var menuListingSearchDTO = new MenuListingSearchRequestDTO();

            // Serialize the data to be posted
            var jsonSearch = JsonSerializer.Serialize(menuListingSearchDTO);
            var data = new StringContent(jsonSearch, Encoding.UTF8, "application/json");

            var baseAddress = new Uri(_configuration.GetValue<string>("Misc:BaseWebApiUrl"));
            var response = String.Empty;

            // Create instance of HttpClientFactory
            var client = _httpClientFactory.CreateClient("LocalClient");
            client.BaseAddress = baseAddress;

            // Add authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage httpResponse = await client.PostAsync("/api/MenuListings/GetMenuListings", data);
            httpResponse.EnsureSuccessStatusCode();
            if (httpResponse.IsSuccessStatusCode)
            {
                response = await httpResponse.Content.ReadAsStringAsync();
            }

            var results = JsonSerializer.Deserialize<List<MenuListingGetResponseDTO>>(response);

            foreach (var item in results)
            {
                indexViewModel.MenuItems.Add(new Models.MenuListing()
                {
                    ItemId = item.ItemId,
                    Name = item.Name,
                    Category = item.Category,
                    Cost = item.Cost
                });
            }
        }

        public async Task ProcessAddToCartRequest(int itemId, int quantity, HttpContext httpContext)
        {
            _logger.LogInformation($"ProcessAddToCartRequest was called with itemId: {itemId}, quantity: {quantity}");

            // get token from the HttpContext so we can add it to the authorization header
            var token = await httpContext.GetTokenAsync("access_token");
            var username = httpContext.User.Identity.Name;

            var cartRequest = new ShoppingCartCreateRequestDTO
            {
                ItemId = itemId,
                Quantity = quantity,
                Username = username
            };

            // Serialize the data to be posted
            var jsonData = JsonSerializer.Serialize(cartRequest);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var baseAddress = new Uri(_configuration.GetValue<string>("Misc:BaseWebApiUrl"));

            // Create instance of HttpClientFactory
            var client = _httpClientFactory.CreateClient("LocalClient");
            client.BaseAddress = baseAddress;

            // Add authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage httpResponse = await client.PostAsync("/api/ShoppingCarts/AddToCart", content);
            httpResponse.EnsureSuccessStatusCode();
        }
    }
}
