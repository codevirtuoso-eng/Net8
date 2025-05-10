using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MvcWebApplication.ViewModels.Shopping;
using SharedLibrary.DTO.MenuListing;
using SharedLibrary.DTO.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MvcWebApplication.ViewFunctions
{
    public class ShoppingViewFunctions : IShoppingViewFunctions
    {
        public async Task<IndexViewModel> ProcessIndexRequest(IndexViewModel model, HttpContext context)
        {
            // Implementation for processing index request
            return model;
        }

        public async Task<bool> ProcessAddToCartRequest(int menuItemId, int quantity, HttpContext context)
        {
            // Implementation for processing add to cart request
            return true;
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
                        Id = dto.Id,
                        Name = dto.Name,
                        Description = dto.Description,
                        Price = dto.Price,
                        Category = dto.Category
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
                    MenuListingId = menuItemId,  // Changed from MenuItemId
                    Quantity = quantity,
                    UserIdentifier = userId  // Changed from UserId
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
                    MenuListingId = menuItemId,  // Changed from MenuItemId
                    UserIdentifier = userId  // Changed from UserId
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
                    UserIdentifier = userId  // Changed from UserId
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