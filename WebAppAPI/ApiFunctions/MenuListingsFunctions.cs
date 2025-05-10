using DatabaseAccess.Data.EntityModels;
using DatabaseAccess.Data.Interfaces;
using Microsoft.Extensions.Logging;
using SharedLibrary.Common.Models;
using SharedLibrary.DTO.MenuListing;
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

			List<MenuListingGetResponseDTO> menuListingList = new List<MenuListingGetResponseDTO>();

			var menulistingSearch = new MenuListingSearch()
			{
				Category = menuListingSearchRequestDTO.Category
			};

			var dbMenuListingList = await _menuListingData.GetMenuListings(menulistingSearch);

			foreach (var item in dbMenuListingList)
			{
				MenuListingGetResponseDTO menuListingGetResponseDTO = new MenuListingGetResponseDTO()
				{
					ItemId = item.ItemId,
					Name = item.Name,
					Category = item.Category,
					Cost = item.Cost
				};
				menuListingList.Add(menuListingGetResponseDTO);
			}

			return menuListingList;
		}

		public async Task<MenuListingCreateResponseDTO> CreateMenuListing(MenuListingCreateRequestDTO menuListingCreateRequestDTO)
		{
			_logger.LogInformation($"CreateMenuListing was called with menuListingCreateRequestDTO: {menuListingCreateRequestDTO}");

			var menuListingDAO = new MenuListingDAO()
			{
				ItemId = menuListingCreateRequestDTO.ItemId,
				Name = menuListingCreateRequestDTO.Name,
				Category = menuListingCreateRequestDTO.Category,
				Cost = menuListingCreateRequestDTO.Cost
			};

			await _menuListingData.CreateMenuListing(menuListingDAO);

			var menuListingCreateResponseDTO = new MenuListingCreateResponseDTO()
			{
				ItemId = menuListingDAO.ItemId
			};

			return menuListingCreateResponseDTO;
		}

		public async Task<MenuListingGetResponseDTO> GetMenuListing(MenuListingGetRequestDTO menuListingGetRequestDTO)
		{
			_logger.LogInformation($"GetMenuListing was called with itemId: {menuListingGetRequestDTO}");
			var menuListingDAO = await _menuListingData.GetMenuListing(menuListingGetRequestDTO.ItemId);

			var menuListingResponseDTO = new MenuListingGetResponseDTO()
			{
				ItemId = menuListingDAO.ItemId,
				Name = menuListingDAO.Name,
				Category = menuListingDAO.Category,
				Cost = menuListingDAO.Cost
			};

			return menuListingResponseDTO;
		}

		public async Task UpdateMenuListing(MenuListingUpdateRequestDTO menuListingUpdateRequestDTO)
		{
			_logger.LogInformation($"UpdateMenuListing was called with menuListingUpdateRequestDTO: {menuListingUpdateRequestDTO}");

			var menuListingDAO = new MenuListingDAO()
			{
				ItemId = menuListingUpdateRequestDTO.ItemId,
				Name = menuListingUpdateRequestDTO.Name,
				Category = menuListingUpdateRequestDTO.Category,
				Cost = menuListingUpdateRequestDTO.Cost
			};

			await _menuListingData.UpdateMenuListing(menuListingDAO);
		}

		public async Task DeleteMenuListing(MenuListingDeleteRequestDTO menuListingDeleteRequestDTO)
		{
			_logger.LogInformation($"DeleteMenuListing was called with menuListingDeleteRequestDTO: {menuListingDeleteRequestDTO}");
			await _menuListingData.DeleteMenuListing(menuListingDeleteRequestDTO.ItemId);
		}
	}
}
