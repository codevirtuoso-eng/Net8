using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.HttpClient;
using MvcWebApplication.Controllers;
using MvcWebApplication.ViewFunctions;
using NUnit.Framework;
using SharedLibrary.DTO.Order;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;
using System.Threading.Tasks;

namespace NUnitTestProject.WebAppTests.OrdersControllerTests
{
	[TestFixture]
	public class Given
	{
		// Mock logger since dependency injection requires it; we really do not want any logging
		protected readonly Mock<ILogger<OrdersController>> MockOrdersControllerLogger = new Mock<ILogger<OrdersController>>();

		// Need to mock the registered HttpClientFactory
		protected readonly Mock<HttpMessageHandler> MockHttpMessageHandler = new Mock<HttpMessageHandler>();
		protected readonly IHttpClientFactory IHttpClientFactory;

		// Need to mock HttpContext
		protected readonly HttpContext FakeHttpContext;

		// Create our own isolated ServiceCollection to register needed services for our tests
		protected readonly ServiceCollection Services;
		protected readonly ServiceProvider ServiceProvider;

		// setups the conditions needed to perform the various tests
		// in this case we want to perform an a true unit test involving only the MVC web app
		// since a HTTP call is external to the MVC web app, we must mock the HTTP call
		public Given()
		{
			// Create in-memory version of IConfiguraton
			var inMemorySettings = new Dictionary<string, string> {
				{"TopLevelKey", "TopLevelValue"},
				{"Misc:BaseWebApiUrl", "https://localhost:5001"}
			};

			// Need to support injection of the configuration manger for the connection string
			IConfiguration configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(inMemorySettings)
				.Build();

			// Create mocked client factory for HttpClient
			IHttpClientFactory = MockHttpMessageHandler.CreateClientFactory();

			var getOrdersUrl = "https://localhost:5001/api/Orders/GetOrders";
			var getCreateOrdersUrl = "https://localhost:5001/api/Orders/CreateOrder";

			// Mocked message handlers based upon specific urls and the model values
			// When a mocked HttpClientFactory using Post is called, the content passed into the call is deserialized
			// then selection is based upon the model's values
			MockHttpMessageHandler
				.SetupRequest(HttpMethod.Post, getOrdersUrl, async request =>
				{
					var json = await request.Content.ReadAsStringAsync();
					var contentModel = JsonSerializer.Deserialize<OrderSearchRequestDTO>(json);
					return (contentModel.UserId == null && contentModel.BeginOrderDate == null && contentModel.EndOrderDate == null);
				})
				.ReturnsResponse(GetAllOrdersTestData());

			MockHttpMessageHandler
				.SetupRequest(HttpMethod.Post, getOrdersUrl, async request =>
				{
					var json = await request.Content.ReadAsStringAsync();
					var contentModel = JsonSerializer.Deserialize<OrderSearchRequestDTO>(json);
					return (contentModel.UserId == "FakeId" && contentModel.BeginOrderDate == null && contentModel.EndOrderDate == null);
				})
				.ReturnsResponse(GetOrdersTestDataWithUserId());

			MockHttpMessageHandler
				.SetupRequest(HttpMethod.Post, getOrdersUrl, async request =>
				{
					var json = await request.Content.ReadAsStringAsync();
					var contentModel = JsonSerializer.Deserialize<OrderSearchRequestDTO>(json);
					return (contentModel.UserId == "FakeId" && contentModel.BeginOrderDate == DateTime.Now.Date && contentModel.EndOrderDate == null);
				})
				.ReturnsResponse(GetOrdersTestDataWithBeginDate());


			MockHttpMessageHandler
				.SetupRequest(HttpMethod.Post, getOrdersUrl, async request =>
				{
					var json = await request.Content.ReadAsStringAsync();
					var contentModel = JsonSerializer.Deserialize<OrderSearchRequestDTO>(json);
					return (contentModel.UserId == "FakeId" && contentModel.BeginOrderDate == DateTime.Now.Date && contentModel.EndOrderDate == DateTime.Now.Date.AddDays(10));
				})
				.ReturnsResponse(GetOrdersTestDataWithBeginAndEndDate());

			MockHttpMessageHandler
				.SetupRequest(HttpMethod.Post, getCreateOrdersUrl, async request =>
				{
					var json = await request.Content.ReadAsStringAsync();
					var contentModel = JsonSerializer.Deserialize<OrderCreateRequestDTO>(json);
					return (contentModel.UserId == "FakeId");
				})
				.ReturnsResponse(HttpStatusCode.OK);

			// Create cookies to be added to HttpContext
			CookieOptions option = new CookieOptions();

			// Generate fake identity to be used with fake HttpContext
			var identity = new GenericIdentity("Joe", "Test"); //add claims as needed
			identity.AddClaim(new Claim(ClaimTypes.Name, "fakeUserName"));
			identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "FakeId"));

			var contextUser = new ClaimsPrincipal(identity);

			// Create fake service provider to allow us to register some fake services to our fake HttpContext
			var serviceProvider = new Mock<IServiceProvider>();

			// Create fake tempDataFactory service to support MVC controller tests
			var tempDataDictionaryFactoryMock = new Mock<ITempDataDictionaryFactory>();

			// Create fake urlHelperFactory service to support MVC controller tests
			var urlHelperFactory = new Mock<IUrlHelperFactory>();

			// Create fake authentication service to allow us to store fake access-token in our fake HttpContext
			var authenticationServiceMock = new Mock<IAuthenticationService>();
			var authResult = AuthenticateResult.Success(
				new AuthenticationTicket(new ClaimsPrincipal(), null));

			authResult.Properties.StoreTokens(new[]
			{
				new AuthenticationToken { Name = "access_token", Value = "fakeAccessToken" }
			});

			authenticationServiceMock
				.Setup(x => x.AuthenticateAsync(It.IsAny<HttpContext>(), null))
				.ReturnsAsync(authResult);

			serviceProvider.Setup(s => s.GetService(typeof(IAuthenticationService))).Returns(authenticationServiceMock.Object);
			serviceProvider.Setup(s => s.GetService(typeof(ITempDataDictionaryFactory))).Returns(tempDataDictionaryFactoryMock.Object);
			serviceProvider.Setup(s => s.GetService(typeof(IUrlHelperFactory))).Returns(urlHelperFactory.Object);

			// Create fake HttpContext using the fake authentication service
			FakeHttpContext = new DefaultHttpContext
			{
				User = contextUser,
				RequestServices = serviceProvider.Object
			};

			// define new service collection to support unit tests
			Services = new ServiceCollection();

			// Create fake httpContextAcessor to support dependency injection for ShoppingViewFunctions
			var accessor = new HttpContextAccessor();
			accessor.HttpContext = FakeHttpContext;

			// Register fake httpContextAcessor service needed for ShoppingViewFunctions dependency injection
			Services.AddSingleton<IHttpContextAccessor>(accessor);

			Services.AddTransient<IOrdersViewFunctions, OrdersViewFunctions>();
			Services.AddTransient<IHomeViewFunctions, HomeViewFunctions>();
			Services.AddSingleton(configuration);

			// Register service for named HttpClientFactory
			Services.AddHttpClient("LocalClient").ConfigurePrimaryHttpMessageHandler(() => MockHttpMessageHandler.Object);

			// Add generic logging to support injection of logging
			Services.AddLogging();

			// Build the service container to hold all of the service dependency registrations
			ServiceProvider = Services.BuildServiceProvider();
		}

		private string GetOrdersTestDataWithUserId()
		{
			var orderDTOList = new List<OrderSearchResponseDTO>
			{
				new OrderSearchResponseDTO{ OrderId="12345", UserId="FakeId", OrderTotal=10.00M, OrderDate=DateTime.Now.AddDays(-3) }           };

			return JsonSerializer.Serialize(orderDTOList);
		}

		private string GetOrdersTestDataWithBeginDate()
		{
			var orderDTOList = new List<OrderSearchResponseDTO>
			{
				new OrderSearchResponseDTO{ OrderId="11111", UserId="FakeId", OrderTotal=20.00M, OrderDate=DateTime.Now.AddDays(3) },
				new OrderSearchResponseDTO{ OrderId="222222", UserId="FakeId", OrderTotal=15.00M, OrderDate=DateTime.Now.AddDays(5) }
			};

			return JsonSerializer.Serialize(orderDTOList);
		}

		private string GetOrdersTestDataWithBeginAndEndDate()
		{
			var orderDTOList = new List<OrderSearchResponseDTO>
			{
				new OrderSearchResponseDTO{ OrderId="00000", UserId="FakeId", OrderTotal=15.00M, OrderDate=DateTime.Now.AddDays(5) },
				new OrderSearchResponseDTO{ OrderId="99999", UserId="FakeId", OrderTotal=10.00M, OrderDate=DateTime.Now.AddDays(-3) },
				new OrderSearchResponseDTO{ OrderId="88888", UserId="FakeId", OrderTotal=20.00M, OrderDate=DateTime.Now.AddDays(3) }
			};

			return JsonSerializer.Serialize(orderDTOList);
		}

		private string GetAllOrdersTestData()
		{
			var orderDTOList = new List<OrderSearchResponseDTO>
			{
				new OrderSearchResponseDTO{ OrderId="00001", UserId="FakeId", OrderTotal=10.00M, OrderDate=DateTime.Now.AddDays(-3) },
				new OrderSearchResponseDTO{ OrderId="00002", UserId="FakeId", OrderTotal=20.00M, OrderDate=DateTime.Now.AddDays(3) },
				new OrderSearchResponseDTO{ OrderId="00003", UserId="FakeId", OrderTotal=15.00M, OrderDate=DateTime.Now.AddDays(5) },
				new OrderSearchResponseDTO{ OrderId="00004", UserId="FakeId", OrderTotal=35.00M, OrderDate=DateTime.Now.AddDays(-5) }
			};

			return JsonSerializer.Serialize(orderDTOList);
		}
	}
}
