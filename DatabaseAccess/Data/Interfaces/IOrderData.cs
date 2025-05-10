using DatabaseAccess.Data.EntityModels;
using SharedLibrary.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccess.Data.Interfaces
{
	public  interface IOrderData
	{
		public Task<List<OrderDAO>> GetOrders(OrderSearch orderSearch);

		public Task<OrderDAO> GetOrder(string orderId, string userId);

		public Task<List<OrderDetailDAO>> GetOrderDetails(string orderId);

		public Task CreateOrder(OrderDAO orderDao, List<OrderDetailDAO> orderDetailDaoList);
	}
}
