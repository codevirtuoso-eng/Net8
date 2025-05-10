using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvcWebApplication.Models;
using MvcWebApplication.ViewModels.ShoppingCarts;
using SharedLibrary.DTO.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace MvcWebApplication.ViewFunctions
{
    public class ShoppingCartViewFunctions : IShoppingCartViewFunctions
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ShoppingCartViewFunctions> _logger;

        public ShoppingCartViewFunctions(
            IHttpClientFactory clientFactory,
            IConfiguration configuration,
            ILogger<ShoppingCartViewFunctions> logger)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IndexViewModel> GetIndexViewModel(string bearerToken, string userId)
        {
            try
            {
                var cartItems = await GetShoppingCartItems(bearerToken, userId);
                decimal cartTotal = 0;

                foreach (var item in cartItems)
                {
                    cartTotal += item.LineTotal;
                }

                var viewModel = new IndexViewModel
                {
                    CartItems = cartItems,
                    CartTotal = cartTotal
                };

                return viewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving shopping cart view model");
                return new IndexViewModel();
            }
        }

        private async Task<List<ShoppingCartItem>> GetShoppingCartItems(string bearerToken, string userId)
        {
            var client = _clientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var requestUri = $"{apiBaseUrl}/api/ShoppingCarts/{userId}";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var response = await client.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var cartDto = JsonSerializer.Deserialize<ShoppingCartGetResponseDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var cartItems = new List<ShoppingCartItem>();

                // Assuming cartDto.Items is the collection of items in the cart
                if (cartDto?.Items != null)
                {
                    foreach (var item in cartDto.Items)
                    {
                        cartItems.Add(new ShoppingCartItem
                        {
                            Id = item.MenuListingId,  // Adjust property name if needed
                            Name = item.Name,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            LineTotal = item.Price * item.Quantity
                        });
                    }
                }

                return cartItems;
            }

            return new List<ShoppingCartItem>();
        }

        public async Task ProcessIndexRequest(IndexViewModel indexViewModel, Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            var token = await httpContext.GetTokenAsync("access_token");
            var username = httpContext.User.Identity.Name;

            var viewModel = await GetIndexViewModel(token, username);

            indexViewModel.CartItems = viewModel.CartItems;
            indexViewModel.TotalAmount = viewModel.CartTotal;
        }

        public async Task ProcessRemoveItemRequest(int itemId, Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            _logger.LogInformation($"ProcessRemoveItemRequest was called with itemId: {itemId}");

            // get token from the HttpContext so we can add it to the authorization header
            var token = await httpContext.GetTokenAsync("access_token");
            var username = httpContext.User.Identity.Name;

            var client = _clientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var requestUri = $"{apiBaseUrl}/api/ShoppingCarts/RemoveItem/{username}/{itemId}";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync(requestUri, null);
            response.EnsureSuccessStatusCode();
        }

        public async Task ProcessEmptyCartRequest(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            _logger.LogInformation($"ProcessEmptyCartRequest was called");

            // get token from the HttpContext so we can add it to the authorization header
            var token = await httpContext.GetTokenAsync("access_token");
            var username = httpContext.User.Identity.Name;

            var client = _clientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var requestUri = $"{apiBaseUrl}/api/ShoppingCarts/EmptyCart/{username}";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync(requestUri, null);
            response.EnsureSuccessStatusCode();
        }

        public async Task ProcessCheckoutRequest(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            _logger.LogInformation($"ProcessCheckoutRequest was called");

            // get token from the HttpContext so we can add it to the authorization header
            var token = await httpContext.GetTokenAsync("access_token");
            var username = httpContext.User.Identity.Name;

            var client = _clientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var requestUri = $"{apiBaseUrl}/api/Orders/CreateOrder/{username}";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync(requestUri, null);
            response.EnsureSuccessStatusCode();
        }
    }
}