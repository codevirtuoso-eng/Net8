using DatabaseAccess.Data.Context;
using DatabaseAccess.Data.EntityModels;
using DatabaseAccess.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedLibrary.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseAccess.Data.DataAccess
{
	public class OrderData : IOrderData
	{
		private readonly MainAppDbContext _mainAppDbContext;
		private readonly ILogger<OrderData> _logger;

		public OrderData(MainAppDbContext mainAppDbContext, ILogger<OrderData> logger)
		{
			_mainAppDbContext = mainAppDbContext;
			_logger = logger;
			_logger.LogDebug("NLog injected into OrderData");
		}

		public async Task<List<OrderDAO>> GetOrders(OrderSearch orderSearch)
		{
			_logger.LogInformation($"GetOrders was called with orderSearch: {orderSearch}");

			var query = _mainAppDbContext.Orders.AsQueryable();

			if (!string.IsNullOrEmpty(orderSearch.UserId))
			{
				query = query.Where(a => a.UserId == orderSearch.UserId);
			}

			if (orderSearch.BeginOrderDate != null)
			{
				query = query.Where(a => a.OrderDate >= orderSearch.BeginOrderDate);
			}

			if (orderSearch.EndOrderDate != null)
			{
				query = query.Where(a => a.OrderDate <= orderSearch.EndOrderDate);
			}

			query = query.OrderByDescending(a => a.OrderDate);

			List<OrderDAO> orderDAOList = await query.ToListAsync<OrderDAO>();

			return orderDAOList;
		}

		public async Task<OrderDAO> GetOrder(string orderId, string userId)
		{
			_logger.LogInformation($"GetOrder was called with orderId: {orderId}");

			throw new System.NotImplementedException();
		}

		public async Task<List<OrderDetailDAO>> GetOrderDetails(string orderId)
		{
			_logger.LogInformation($"GetOrderDetails was called with orderId: {orderId}");

			throw new System.NotImplementedException();
		}

		public async Task CreateOrder(OrderDAO orderDao, List<OrderDetailDAO> orderDetailDaoList)
		{
			_logger.LogInformation($"GetOrder was called with orderDao: {orderDao} orderDetailDaoList.Count: {orderDetailDaoList.Count}");

			// The code should be written to perform a single transaction
			// In other words, make all of the LINQ calls before calling SaveChanges()
			throw new System.NotImplementedException();
		}
	}
}
