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

		public async Task AddToCart(ShoppingCartCreateRequestDTO shoppingCartCreateRequestDTO)
		{
			_logger.LogInformation($"AddToCart was called with shoppingCartCreateRequestDTO: {shoppingCartCreateRequestDTO}");

			// Validate the request
			if (shoppingCartCreateRequestDTO == null || string.IsNullOrEmpty(shoppingCartCreateRequestDTO.UserId))
			{
				throw new System.ArgumentException("Invalid shopping cart request");
			}

			// First, check if the menu item exists
			var menuItem = await _menuListingData.GetMenuListing(shoppingCartCreateRequestDTO.ItemId);
			if (menuItem == null)
			{
				throw new System.ArgumentException("Menu item not found");
			}

			// Add to cart
			await _shoppingCartData.AddItemToCart(shoppingCartCreateRequestDTO.UserId, shoppingCartCreateRequestDTO.ItemId);
		}

		public async Task RemoveFromCart(ShoppingCartRemoveRequestDTO shoppingCartRemoveRequestDTO)
		{
			_logger.LogInformation($"RemoveFromCart was called with shoppingCartRemoveRequestDTO: {shoppingCartRemoveRequestDTO}");

			// Validate the request
			if (shoppingCartRemoveRequestDTO == null || string.IsNullOrEmpty(shoppingCartRemoveRequestDTO.UserId))
			{
				throw new System.ArgumentException("Invalid shopping cart request");
			}

			await _shoppingCartData.RemoveItemFromCart(shoppingCartRemoveRequestDTO.UserId, shoppingCartRemoveRequestDTO.CartId);
		}

		public async Task EmptyCart(ShoppingCartEmptyRequestDTO shoppingCartEmptyRequestDTO)
		{
			_logger.LogInformation($"EmptyCart was called with shoppingCartEmptyRequestDTO: {shoppingCartEmptyRequestDTO}");

			// Validate the request
			if (shoppingCartEmptyRequestDTO == null || string.IsNullOrEmpty(shoppingCartEmptyRequestDTO.UserId))
			{
				throw new System.ArgumentException("Invalid shopping cart request");
			}

			await _shoppingCartData.EmptyCart(shoppingCartEmptyRequestDTO.UserId);
		}
	}
}