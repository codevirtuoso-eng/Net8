
using Microsoft.AspNetCore.Http;
using MvcWebApplication.ViewModels.Orders;
using System.Threading.Tasks;

namespace MvcWebApplication.ViewFunctions
{
	public interface IOrdersViewFunctions
	{
		Task GetOrders(IndexViewModel IndexViewModel, HttpContext httpContext);
		Task CreateOrder(string userId, HttpContext httpContext);
		Task GetOrderDetails(string orderId, string userId, GetOrderDetailsViewModel getOrderDetailsViewModel, HttpContext httpContext);
		Task<OrdersGetOrderDetailsViewModel> GetOrderDetailsViewModel(string orderId, string userId, HttpContext httpContext);
		Task GetUserOrders(UserOrdersViewModel userOrdersViewModel, HttpContext httpContext);
	}
}
