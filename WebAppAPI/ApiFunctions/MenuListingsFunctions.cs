
using DatabaseAccess.Data.EntityModels;
using DatabaseAccess.Data.Interfaces;
using Microsoft.Extensions.Logging;
using SharedLibrary.Common.Models;
using SharedLibrary.DTO.MenuListing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAppAPI.ApiFunctions
{
	public class MenuListingsFunctions : IMenuListingsFunctions
	{
		private IMenuListingData _menuListingData;
		private readonly ILogger<MenuListingsFunctions> _logger;

		public MenuListingsFunctions(IMenuListingData menuListingData, ILogger<MenuListingsFunctions> logger)
		{
			_menuListingData = menuListingData;
			_logger = logger;
			_logger.LogDebug("NLog injected into MenuListingsFunctions");
		}

		public async Task<List<MenuListingGetResponseDTO>> GetMenuListings(MenuListingSearchRequestDTO menuListingSearchRequestDTO)
		{
			_logger.LogInformation($"GetMenuListings was called with menuListingSearchRequestDTO: {menuListingSearchRequestDTO}");

			var menuListingSearch = new MenuListingSearch()
			{
				Category = menuListingSearchRequestDTO.Category,
				Name = menuListingSearchRequestDTO.Name
			};

			var menuListingsDAO = await _menuListingData.GetMenuListings(menuListingSearch);

			List<MenuListingGetResponseDTO> menuListingDTOs = new List<MenuListingGetResponseDTO>();

			foreach (var item in menuListingsDAO)
			{
				var menuListing = new MenuListingGetResponseDTO()
				{
					ItemId = item.ItemId,
					Name = item.Name,
					Description = item.Description,
					Category = item.Category,
					Cost = item.Cost,
					ImageUrl = item.ImageUrl
				};

				menuListingDTOs.Add(menuListing);
			}

			return menuListingDTOs;
		}

		public async Task<MenuListingGetResponseDTO> GetMenuListing(MenuListingGetRequestDTO menuListingGetRequestDTO)
		{
			_logger.LogInformation($"GetMenuListing was called with menuListingGetRequestDTO: {menuListingGetRequestDTO}");

			var menuListingDAO = await _menuListingData.GetMenuListing(menuListingGetRequestDTO.ItemId);

			if (menuListingDAO == null)
			{
				return null;
			}

			var menuListing = new MenuListingGetResponseDTO()
			{
				ItemId = menuListingDAO.ItemId,
				Name = menuListingDAO.Name,
				Description = menuListingDAO.Description,
				Category = menuListingDAO.Category,
				Cost = menuListingDAO.Cost,
				ImageUrl = menuListingDAO.ImageUrl
			};

			return menuListing;
		}

		public async Task<MenuListingCreateResponseDTO> CreateMenuListing(MenuListingCreateRequestDTO menuListingCreateRequestDTO)
		{
			_logger.LogInformation($"CreateMenuListing was called with menuListingCreateRequestDTO: {menuListingCreateRequestDTO}");

			var menuListingDAO = new MenuListingDAO()
			{
				Name = menuListingCreateRequestDTO.Name,
				Description = menuListingCreateRequestDTO.Description,
				Category = menuListingCreateRequestDTO.Category,
				Cost = menuListingCreateRequestDTO.Cost,
				ImageUrl = menuListingCreateRequestDTO.ImageUrl
			};

			var result = await _menuListingData.CreateMenuListing(menuListingDAO);

			var response = new MenuListingCreateResponseDTO()
			{
				ItemId = result.ItemId,
				Name = result.Name,
				Description = result.Description,
				Category = result.Category,
				Cost = result.Cost,
				ImageUrl = result.ImageUrl
			};

			return response;
		}

		public async Task UpdateMenuListing(MenuListingUpdateRequestDTO menuListingUpdateRequestDTO)
		{
			_logger.LogInformation($"UpdateMenuListing was called with menuListingUpdateRequestDTO: {menuListingUpdateRequestDTO}");

			// First check if the menu listing exists
			var existingMenuItem = await _menuListingData.GetMenuListing(menuListingUpdateRequestDTO.ItemId);
			if (existingMenuItem == null)
			{
				throw new ArgumentException($"Menu item with ID {menuListingUpdateRequestDTO.ItemId} not found");
			}

			var menuListingDAO = new MenuListingDAO()
			{
				ItemId = menuListingUpdateRequestDTO.ItemId,
				Name = menuListingUpdateRequestDTO.Name,
				Description = menuListingUpdateRequestDTO.Description,
				Category = menuListingUpdateRequestDTO.Category,
				Cost = menuListingUpdateRequestDTO.Cost,
				ImageUrl = menuListingUpdateRequestDTO.ImageUrl
			};

			await _menuListingData.UpdateMenuListing(menuListingDAO);
		}

		public async Task DeleteMenuListing(MenuListingDeleteRequestDTO menuListingDeleteRequestDTO)
		{
			_logger.LogInformation($"DeleteMenuListing was called with menuListingDeleteRequestDTO: {menuListingDeleteRequestDTO}");

			// First check if the menu listing exists
			var existingMenuItem = await _menuListingData.GetMenuListing(menuListingDeleteRequestDTO.ItemId);
			if (existingMenuItem == null)
			{
				throw new ArgumentException($"Menu item with ID {menuListingDeleteRequestDTO.ItemId} not found");
			}

			await _menuListingData.DeleteMenuListing(menuListingDeleteRequestDTO.ItemId);
		}
	}
}
