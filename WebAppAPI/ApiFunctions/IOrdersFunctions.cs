using SharedLibrary.DTO.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAppAPI.ApiFunctions
{
	public interface IOrdersFunctions
	{
		 public Task<List<OrderSearchResponseDTO>> GetOrders(OrderSearchRequestDTO searchDTO);

		public Task<OrderGetResponseDTO> GetOrderDetails(OrderGetRequestDTO orderGetRequestDTO);

		public Task CreateOrder(OrderCreateRequestDTO orderCreateRequestDTO);
	}
}
