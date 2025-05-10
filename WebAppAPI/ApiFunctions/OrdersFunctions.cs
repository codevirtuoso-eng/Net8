using DatabaseAccess.Data.EntityModels;
using DatabaseAccess.Data.Interfaces;
using Microsoft.Extensions.Logging;
using SharedLibrary.Common.Models;
using SharedLibrary.DTO.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAppAPI.ApiFunctions
{
	public class OrdersFunctions : IOrdersFunctions
	{
		private IOrderData _orderData;
		private readonly ILogger<OrdersFunctions> _logger;

		public OrdersFunctions(IOrderData orderData, ILogger<OrdersFunctions> logger)
		{
			_orderData = orderData;
			_logger = logger;
			_logger.LogDebug("NLog injected into OrdersFunctions");
		}


		public async Task<List<OrderSearchResponseDTO>> GetOrders(OrderSearchRequestDTO orderSearchDTO)
		{
			_logger.LogInformation($"GetOrders was called with orderSearchDTO: {orderSearchDTO}");

			// Not good practice to send DTOs to other layers => separation of concerns
			// Convert DTO to data layer object
			var orderSearch = new OrderSearch()
			{
				UserId = orderSearchDTO.UserId,
				BeginOrderDate = orderSearchDTO.BeginOrderDate,
				EndOrderDate = orderSearchDTO.EndOrderDate
			};

			List<OrderDAO> orderDataList = await _orderData.GetOrders(orderSearch);

			List<OrderSearchResponseDTO> dtoList = new List<OrderSearchResponseDTO>();

			// Not a good practice to pass lower level objects when returning web API data - separation of concerns
			// Convert DAO data to DTO data
			foreach (var daoData in orderDataList)
			{
				var dtoObj = new OrderSearchResponseDTO();
				dtoObj.OrderId = daoData.OrderId;
				dtoObj.UserId = daoData.UserId;
				dtoObj.OrderTotal = daoData.OrderTotal;
				dtoObj.OrderDate = daoData.OrderDate;
				dtoList.Add(dtoObj);
			}

			return dtoList;
		}

		public Task<OrderGetResponseDTO> GetOrderDetails(OrderGetRequestDTO orderGetRequestDTO)
		{
			throw new System.NotImplementedException();
		}

		public Task CreateOrder(OrderCreateRequestDTO orderCreateRequestDTO)
		{
			throw new System.NotImplementedException();
		}
	}
}
