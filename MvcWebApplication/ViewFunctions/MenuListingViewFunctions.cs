using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvcWebApplication.Models;
using MvcWebApplication.ViewModels.MenuListings;
using SharedLibrary.DTO.MenuListing;
using SharedLibrary.Enums;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;

namespace MvcWebApplication.ViewFunctions
{
    public class MenuListingViewFunctions : IMenuListingViewFunctions
    {
        private readonly ILogger<MenuListingViewFunctions> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public MenuListingViewFunctions(ILogger<MenuListingViewFunctions> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
            _logger.LogDebug("NLog injected into MenuListingViewFunctions");
        }

        public async Task ProcessIndexRequest(IndexViewModel indexViewModel, HttpContext httpContext)
        {
            _logger.LogInformation($"ProcessIndexRequest was called with indexViewModel: {indexViewModel}");

            // get token from the HttpContext so we can add it to the authorization header
            var token = httpContext.GetTokenAsync("access_token").Result;
            var user = httpContext.User;

            // Not good practice to pass MVC model to web API - separation of concerns
            // Convert orderSearchViewModel.OrderSearch to OrderSearchDTO
            var menuListingSearchDTO = new MenuListingSearchRequestDTO()
            {
                Category = indexViewModel.MenuListingSearch.Category
            };

            // Serialize the data to be posted
            var jsonSearch = JsonSerializer.Serialize(menuListingSearchDTO);
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

            HttpResponseMessage httpResponse = await client.PostAsync("/api/MenuListings/GetMenuListings", data);
            httpResponse.EnsureSuccessStatusCode();
            if (httpResponse.IsSuccessStatusCode)
            {
                response = await httpResponse.Content.ReadAsStringAsync();
            }

            var results = JsonSerializer.Deserialize<List<MenuListingGetResponseDTO>>(response);


            foreach (var item in results)
            {
                indexViewModel.MenuListingList.Add(new MenuListing()
                {
                    ItemId = item.ItemId,
                    Name = item.Name,
                    Category = item.Category,
                    Cost = item.Cost
                });
            }

            var menuCategories = Enum.GetNames(typeof(MenuCategories)).ToList();
            foreach (var item in menuCategories)
            {
                indexViewModel.CategoryList.Add(new SelectListItem(item, item));
            }
        }

        public async Task ProcessSearchRequest(SearchViewModel searchViewModel, HttpContext httpContext)
        {
            _logger.LogInformation($"ProcessIndexRequest was called with searchViewModel: {searchViewModel}");

            // get token from the HttpContext so we can add it to the authorization header
            var token = httpContext.GetTokenAsync("access_token").Result;
            var user = httpContext.User;

            // Not good practice to pass MVC model to web API - separation of concerns
            // Convert orderSearchViewModel.OrderSearch to OrderSearchDTO
            var menuListingSearchDTO = new MenuListingSearchRequestDTO()
            {
                Category = searchViewModel.MenuListingSearch.Category
            };

            // Serialize the data to be posted
            var jsonSearch = JsonSerializer.Serialize(menuListingSearchDTO);
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

            HttpResponseMessage httpResponse = await client.PostAsync("/api/MenuListings/GetMenuListings", data);
            httpResponse.EnsureSuccessStatusCode();
            if (httpResponse.IsSuccessStatusCode)
            {
                response = await httpResponse.Content.ReadAsStringAsync();
            }

            var results = JsonSerializer.Deserialize<List<MenuListingGetResponseDTO>>(response);


            foreach (var item in results)
            {
                searchViewModel.MenuListingList.Add(new MenuListing()
                {
                    ItemId = item.ItemId,
                    Name = item.Name,
                    Category = item.Category,
                    Cost = item.Cost
                });
            }

            var menuCategories = Enum.GetNames(typeof(MenuCategories)).ToList();
            foreach (var item in menuCategories)
            {
                searchViewModel.CategoryList.Add(new SelectListItem(item, item));
            }
        }
    }
}
