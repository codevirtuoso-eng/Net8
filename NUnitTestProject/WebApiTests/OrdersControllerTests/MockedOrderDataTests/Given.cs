using DatabaseAccess.Data.EntityModels;
using DatabaseAccess.Data.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SharedLibrary.Common.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using WebAppAPI.ApiFunctions;
using WebAppAPI.Apis;

namespace NUnitTestProject.WebApiTests.OrdersControllerTests.MockedOrderDataTests
{
	[TestFixture]
	public class Given
	{
		// Mock logger since dependency injection requires it; we really do not want any logging
		protected readonly Mock<ILogger<OrdersController>> MockOrdersControllerLogger = new Mock<ILogger<OrdersController>>();
		
		// Mock OrderData since we really do not want to call the database functions
		protected readonly Mock<IOrderData> MockOrderData = new Mock<IOrderData>();

		// Need to mock HttpContext
		protected readonly HttpContext FakeHttpContext;

		// Create our own isolated ServiceCollection to register needed services for our tests
		protected readonly ServiceCollection Services;
		protected readonly ServiceProvider ServiceProvider;

		// setups the conditions needed to perform the various tests
		// in this case we want to perform an a true unit test involving only the web API
		// since it is only a web API unit test, we will mock the call to the data access class
		public Given() 
		{
			// Return all orders when calling OrderData.GetOrders(OrderSearch orderSearch) when begin and end dates are null
			MockOrderData.Setup(x => x.GetOrders(It.Is<OrderSearch>(x => x.BeginOrderDate == null && x.EndOrderDate == null))).Returns(Task.FromResult(GetAllOrdersTestData()));

			// Return a single order when calling OrderData.GetOrders(OrderSearch orderSearch) when begin date is Now and end date is null
			MockOrderData.Setup(x => x.GetOrders(It.Is<OrderSearch>(x => x.BeginOrderDate != null && x.BeginOrderDate.Value.Date == DateTime.Now.Date && x.EndOrderDate == null))).Returns(Task.FromResult(GetOrdersTestDataWithBeginDate()));

			// Return no orders when calling OrderData.GetOrders(OrderSearch orderSearch) when begin date is Now + 10 days and end date is null
			MockOrderData.Setup(x => x.GetOrders(It.Is<OrderSearch>(x => x.BeginOrderDate != null && x.BeginOrderDate.Value.Date == DateTime.Now.AddDays(10).Date && x.EndOrderDate == null))).Returns(Task.FromResult(GetOrdersTestDataWithWithEmptyList()));

			Services = new ServiceCollection();
			Services.AddTransient<IOrdersFunctions, OrdersFunctions>();

			// Add mock service to the service collection to be used for injection in place of IOrderData
			Services.AddSingleton(MockOrderData.Object);

			// Add generic logging to support injection of logging
			Services.AddLogging();

			// Build the service container to hold all of the service dependency registrations
			ServiceProvider = Services.BuildServiceProvider();

			// Generate fake HttpContext
			var identity = new GenericIdentity("Joe", "Test");
			var contextUser = new ClaimsPrincipal(identity); //add claims as needed

			// Create fake authentication service to allow us to store fake access-token
			var serviceProvider = new Mock<IServiceProvider>();
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

			serviceProvider.Setup(_ => _.GetService(typeof(IAuthenticationService))).Returns(authenticationServiceMock.Object);

			// Create fake HttpCOntext using the fake authentication service
			FakeHttpContext = new DefaultHttpContext
			{
				User = contextUser,
				RequestServices = serviceProvider.Object
			};
		}

		private List<OrderDAO> GetAllOrdersTestData()
		{
			var orderList = new List<OrderDAO>()
			{
				new OrderDAO{ OrderId="12345", UserId="FakeId", OrderTotal=10.00M, OrderDate=DateTime.Now.AddDays(-3) },
				new OrderDAO{ OrderId="78900", UserId="FakeId", OrderTotal=20.00M, OrderDate=DateTime.Now.AddDays(3) }
			};

			return orderList;
		}

		private List<OrderDAO> GetOrdersTestDataWithBeginDate()
		{
			var orderList = new List<OrderDAO>
			{
				new OrderDAO{ OrderId="78900", UserId="FakeId", OrderTotal=20.00M, OrderDate=DateTime.Now.AddDays(3) }
			};

			return orderList;
		}

		private List<OrderDAO> GetOrdersTestDataWithWithEmptyList()
		{
			var orderList = new List<OrderDAO>();

			return orderList;
		}
	}
}
