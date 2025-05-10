
using SharedLibrary.DTO.ShoppingCart;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAppAPI.ApiFunctions
{
	public interface IShoppingCartFunctions
	{
		Task<List<ShoppingCartGetResponseDTO>> GetShoppingCart(ShoppingCartSearchRequestDTO shoppingCartSearchRequestDTO);
		Task AddToCart(ShoppingCartCreateRequestDTO shoppingCartCreateRequestDTO);
		Task RemoveFromCart(ShoppingCartRemoveRequestDTO shoppingCartRemoveRequestDTO);
		Task EmptyCart(ShoppingCartEmptyRequestDTO shoppingCartEmptyRequestDTO);
	}
}
