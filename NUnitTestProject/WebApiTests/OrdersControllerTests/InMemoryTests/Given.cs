using DatabaseAccess.Data.Context;
using DatabaseAccess.Data.DataAccess;
using DatabaseAccess.Data.EntityModels;
using DatabaseAccess.Data.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using WebAppAPI.ApiFunctions;
using WebAppAPI.Apis;

namespace UnitTestProject.WebApiTests.OrdersControllerTests.InMemoryTests
{
	[TestFixture]
	public class Given
	{
		// Mock logger since dependency injection requires it; we really do not want any logging
		protected readonly Mock<ILogger<OrdersController>> MockOrdersControllerLogger = new Mock<ILogger<OrdersController>>();

		// Need to mock HttpContext
		protected readonly HttpContext FakeHttpContext;

		// DbContext used for accessing in-memory data
		protected readonly DbContextOptions<MainAppDbContext> MainAppDbContextOptions;
		protected readonly MainAppDbContext MainAppDbContext;

		// Create our own isolated ServiceCollection to register needed services for our tests
		protected readonly ServiceCollection Services;
		protected readonly ServiceProvider ServiceProvider;

		private List<OrderDAO> OrderList; // will hold in memory data to perform data access tests
		private List<OrderDetailDAO> OrderDetailList;
		private List<ShoppingCartDAO> ShoppingCartList;

		// setups the conditions needed to perform the various tests
		// in this case we want to perform an integration test involving the web API and the data access layer
		// since the actual database is external to the web API, we must mock the database
		// in this case we mock it using an in-memory database
		public Given()
		{
			MainAppDbContextOptions = new DbContextOptionsBuilder<MainAppDbContext>()
				.UseInMemoryDatabase(databaseName: "InMemoryAppDb")
				.Options;

			MainAppDbContext = new MainAppDbContext(MainAppDbContextOptions);

			PopulateOrdersList();
			PopulateOrderDetailsList();
			PopulateShoppingCartList();

			Services = new ServiceCollection();
			Services.AddTransient<IOrdersFunctions, OrdersFunctions>();
			Services.AddTransient<IOrderData, OrderData>();
			Services.AddTransient<IShoppingCartFunctions, ShoppingCartFunctions>();
			Services.AddTransient<IShoppingCartData, ShoppingCartData>();

			// Add service for in-memory DB service and default logging service
			Services.AddEntityFrameworkInMemoryDatabase();
			// Add generic logging to support injection of logging
			Services.AddLogging();

			// Add our in-memory DB Context to the services so it can be injected
			Services.AddDbContext<MainAppDbContext>((y) => y.UseInMemoryDatabase("InMemoryAppDb"),
				ServiceLifetime.Transient);

			// Build the service container to hold all of the service dependency registrations
			ServiceProvider = Services.BuildServiceProvider();

			// Generate fake HttpContext
			var identity = new GenericIdentity("Joe", "Test");
			var contextUser = new ClaimsPrincipal(identity); //add claims as needed

			// Create fake authentication service to allow us to store fake access-token
			var serviceProvider = new Mock<IServiceProvider>();

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

			// Create fake HttpCOntext using the fake authentication service
			FakeHttpContext = new DefaultHttpContext
			{
				User = contextUser,
				RequestServices = serviceProvider.Object
			};
		}

		private void PopulateShoppingCartList()
		{
			// Create list to be added to in-memory database table
			ShoppingCartList = new List<ShoppingCartDAO>
			{
				new ShoppingCartDAO{ CartId=1, UserId="FakeId", ItemId=1, Category="Side", Name="Iced Tea", Cost=1.50M },
				new ShoppingCartDAO{ CartId=2, UserId="FakeId", ItemId=2, Category="Sandwich", Name="Cheeseburger", Cost=10.95M }
			};

			// Add list to the DbContext
			MainAppDbContext.AddRange(ShoppingCartList);
			MainAppDbContext.SaveChanges();
		}

		private void PopulateOrdersList()
		{
			// Create list to be added to in-memory database table
			OrderList = new List<OrderDAO>
			{
				new OrderDAO{ OrderId="12345", UserId="FakeId", OrderTotal=10.00M, OrderDate=DateTime.Now.AddDays(-3) },
				new OrderDAO{ OrderId="78900", UserId="FakeId", OrderTotal=20.00M, OrderDate=DateTime.Now.AddDays(3) }
			};

			// Add list to the DbContext
			MainAppDbContext.AddRange(OrderList);
			MainAppDbContext.SaveChanges();
		}

		private void PopulateOrderDetailsList()
		{
			// Create list to be added to in-memory database table
			OrderDetailList = new List<OrderDetailDAO>();
			OrderDetailList.Add(new OrderDetailDAO
			{
				OrderId = "12345",
				OrderDetailId = "A1",
				ItemId = 1001,
				Category = "Sandwich",
				Name = "Cheeseburger",
				Cost = 8.00M
			});
			OrderDetailList.Add(new OrderDetailDAO
			{
				OrderId = "78900",
				OrderDetailId = "A2",
				ItemId = 1002,
				Category = "Sandwich",
				Name = "Hamburger",
				Cost = 7.00M
			});

			// Add list to the DbContext
			MainAppDbContext.AddRange(OrderDetailList);
			MainAppDbContext.SaveChanges();
		}
	}
}
