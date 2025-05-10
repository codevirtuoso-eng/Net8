using DatabaseAccess.Data.Interfaces;
using Microsoft.Extensions.Logging;
using SharedLibrary.Common.Models;
using SharedLibrary.DTO.ShoppingCart;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAppAPI.ApiFunctions
{
	public class ShoppingCartFunctions : IShoppingCartFunctions
	{
		private IShoppingCartData _shoppingCartData;
		private IMenuListingData _menuListingData;
		private readonly ILogger<ShoppingCartFunctions> _logger;

		public ShoppingCartFunctions(IShoppingCartData shoppingCartData, IMenuListingData menuListingData, ILogger<ShoppingCartFunctions> logger)
		{
			_shoppingCartData = shoppingCartData;
			_menuListingData = menuListingData;
			_logger = logger;
			_logger.LogDebug("NLog injected into ShoppingCartFunctions");
		}

		public async Task<List<ShoppingCartGetResponseDTO>> GetShoppingCart(ShoppingCartSearchRequestDTO shoppingCartSearchRequestDTO)
		{
			_logger.LogInformation($"GetShoppingCart was called with shoppingCartSearchRequestDTO: {shoppingCartSearchRequestDTO}");

			var shoppingCartSearch = new ShoppingCartSearch()
			{
				UserId = shoppingCartSearchRequestDTO.UserId,
				Category = shoppingCartSearchRequestDTO.Category
			};

			var dbShoppingCartList = await _shoppingCartData.GetShoppingCart(shoppingCartSearch);

			List<ShoppingCartGetResponseDTO> shoppingCartList = new List<ShoppingCartGetResponseDTO>();

			foreach (var item in dbShoppingCartList)
			{
				var shoppingCartGetResponseDTO = new ShoppingCartGetResponseDTO()
				{
					CartId = item.CartId,
					UserId = item.UserId,
					ItemId = item.ItemId,
					Name = item.Name,
					Category = item.Category,
					Cost = item.Cost
				};

				shoppingCartList.Add(shoppingCartGetResponseDTO);
			}

			return shoppingCartList;
		}
	}
}
