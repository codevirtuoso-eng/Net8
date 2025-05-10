using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvcWebApplication.Models;
using MvcWebApplication.ViewModels.Shopping;
using SharedLibrary.DTO.MenuListing;
using SharedLibrary.DTO.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace MvcWebApplication.ViewFunctions
{
    public class ShoppingViewFunctions : IShoppingViewFunctions
    {
        public async Task<IndexViewModel> ProcessIndexRequest(IndexViewModel model, HttpContext context)
        {
            try 
            {
                // Implementation for processing index request
                // Use proper model population from APIs
                var client = _clientFactory.CreateClient();
                // Add implementation based on your requirements
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing index request");
                return model;
            }
        }

        public async Task<bool> ProcessAddToCartRequest(int menuItemId, int quantity, HttpContext context)
        {
            try
            {
                // Implementation for processing add to cart request
                // Add HTTP requests to your API endpoints
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart");
                return false;
            }
        }
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ShoppingViewFunctions> _logger;

        public ShoppingViewFunctions(
            IHttpClientFactory clientFactory,
            IConfiguration configuration,
            ILogger<ShoppingViewFunctions> logger)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IndexViewModel> GetIndexViewModel(string bearerToken)
        {
            try
            {
                var menuListings = await GetMenuListings(bearerToken);
                var viewModel = new IndexViewModel
                {
                    MenuListings = menuListings
                };

                return viewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving shopping index view model");
                return new IndexViewModel();
            }
        }

        private async Task<List<MenuListing>> GetMenuListings(string bearerToken)
        {
            var client = _clientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var requestUri = $"{apiBaseUrl}/api/MenuListings";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var response = await client.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dtos = JsonSerializer.Deserialize<List<MenuListingGetResponseDTO>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var menuListings = new List<MenuListing>();
                foreach (var dto in dtos)
                {
                    menuListings.Add(new MenuListing
                    {
                        ItemId = dto.ItemId,
                        Name = dto.Name,
                        Cost = dto.Cost,
                        Category = dto.Category,
                        // Set defaults for properties not in DTO
                        Id = dto.ItemId, // Use ItemId as Id
                        Description = dto.Name, // Use Name as default description
                        Price = dto.Cost // Use Cost as Price
                    });
                }

                return menuListings;
            }

            return new List<MenuListing>();
        }

        public async Task<bool> AddItemToCart(int menuItemId, int quantity, string userId, string bearerToken)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var apiBaseUrl = _configuration["ApiBaseUrl"];
                var requestUri = $"{apiBaseUrl}/api/ShoppingCarts";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                var createDto = new ShoppingCartCreateRequestDTO
                {
                    MenuItemId = menuItemId,
                    Quantity = quantity,
                    UserId = userId
                };

                var json = JsonSerializer.Serialize(createDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(requestUri, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart");
                return false;
            }
        }

        public async Task<bool> RemoveItemFromCart(int menuItemId, string userId, string bearerToken)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var apiBaseUrl = _configuration["ApiBaseUrl"];
                var requestUri = $"{apiBaseUrl}/api/ShoppingCarts/remove";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                var removeDto = new ShoppingCartRemoveRequestDTO
                {
                    MenuItemId = menuItemId,
                    UserId = userId
                };

                var json = JsonSerializer.Serialize(removeDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(requestUri, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from cart");
                return false;
            }
        }

        public async Task<bool> EmptyCart(string userId, string bearerToken)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var apiBaseUrl = _configuration["ApiBaseUrl"];
                var requestUri = $"{apiBaseUrl}/api/ShoppingCarts/empty";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                var emptyDto = new ShoppingCartEmptyRequestDTO
                {
                    UserId = userId
                };

                var json = JsonSerializer.Serialize(emptyDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(requestUri, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error emptying cart");
                return false;
            }
        }
    }
}