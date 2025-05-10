using Microsoft.AspNetCore.Http;
using MvcWebApplication.ViewModels.Orders;
using System.Threading.Tasks;

namespace MvcWebApplication.ViewFunctions
{
    public interface IOrdersViewFunctions
    {
		public Task GetOrders(IndexViewModel indexViewModel, HttpContext httpContext);

		public Task CreateOrder(string userId, HttpContext httpContext);

		public Task GetUserOrders(UserOrdersViewModel userOrdersViewModel, HttpContext httpContext);

		public Task GetOrderDetails(string orderId, string userId, GetOrderDetailsViewModel getOrderDetailsViewModel, HttpContext httpContext);
	}
}
