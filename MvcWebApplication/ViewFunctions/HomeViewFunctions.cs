using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvcWebApplication.ViewModels.Home;
using MvcWebApplication.ViewModels.Orders;
using SharedLibrary.DTO;
using SharedLibrary.DTO.AspNetUser;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MvcWebApplication.ViewFunctions
{
	public class HomeViewFunctions : IHomeViewFunctions
	{
		private readonly ILogger<HomeViewFunctions> _logger;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public HomeViewFunctions(ILogger<HomeViewFunctions> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
		{
			_httpClientFactory = httpClientFactory;
			_logger = logger;
			_configuration = configuration;
			_httpContextAccessor = httpContextAccessor;
			_logger.LogDebug("NLog injected into HomeViewFunctions");
		}

		public async Task Login(HomeLoginViewModel homeLoginViewModel)
		{
			_logger.LogInformation($"Login was called with homeLoginViewModel: {homeLoginViewModel.ToString()}");

			// Access HttpContext via injected IHttpContextAccessor
			var user = _httpContextAccessor.HttpContext.User;

			var baseAddress = new Uri(_configuration.GetValue<string>("Misc:BaseWebApiUrl"));
			var response = String.Empty; // no ""

			// Create instance of HttpClientFacory
			var client = _httpClientFactory.CreateClient("LocalClient");
			client.BaseAddress = baseAddress;

			var loginRequestDTO = new LoginRequestDTO()
			{
				UserName = homeLoginViewModel.UserLogin.UserName,
				UserPassword = homeLoginViewModel.UserLogin.UserPassword
			};

			// Serialize the data to be posted
			var jsonSearch = JsonSerializer.Serialize(loginRequestDTO);
			var data = new StringContent(jsonSearch, Encoding.UTF8, "application/json");

			HttpResponseMessage httpResponse = await client.PostAsync("/api/Users/Login", data);
			httpResponse.EnsureSuccessStatusCode();
			if (httpResponse.IsSuccessStatusCode)
			{
				response = await httpResponse.Content.ReadAsStringAsync();
			}

			var results = JsonSerializer.Deserialize<AspNetUserResponseDTO>(response);

			if (results == null || (results.Id == null && results.UserName == null))
			{
				homeLoginViewModel.Message = "UserName and/or UserPassword are incorrect.";
				return; // return since authentication failed, cannot set cookies
			}

			_httpContextAccessor.HttpContext.Response.Cookies.Append("X-Access-Token", results.Token);
			_httpContextAccessor.HttpContext.Response.Cookies.Append("X-Username", results.UserName);

			homeLoginViewModel.UserLogin.IsAuthenticated = true;
			return;
		}

		public async Task<List<AspNetUserResponseDTO>> GetAllUsers(HttpContext httpContext)
		{
			throw new NotImplementedException();
		}
	}
}
