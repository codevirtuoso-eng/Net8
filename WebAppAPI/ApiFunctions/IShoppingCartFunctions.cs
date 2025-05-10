using SharedLibrary.DTO.ShoppingCart;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAppAPI.ApiFunctions
{
	public interface IShoppingCartFunctions
	{
		public Task<List<ShoppingCartGetResponseDTO>> GetShoppingCart(ShoppingCartSearchRequestDTO shoppingCartSearchRequestDTO);

	}
}
