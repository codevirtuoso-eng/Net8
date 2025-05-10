using SharedLibrary.DTO.MenuListing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAppAPI.ApiFunctions
{
	public interface IMenuListingsFunctions
	{
		public Task<List<MenuListingGetResponseDTO>> GetMenuListings(MenuListingSearchRequestDTO menuListingSearchRequestDTO);
		public Task<MenuListingCreateResponseDTO> CreateMenuListing(MenuListingCreateRequestDTO menuListingCreateRequestDTO);
		public Task<MenuListingGetResponseDTO> GetMenuListing(MenuListingGetRequestDTO menuListingGetRequestDTO);
		public Task UpdateMenuListing(MenuListingUpdateRequestDTO menuListingUpdateRequestDTO);
		public Task DeleteMenuListing(MenuListingDeleteRequestDTO menuListingDeleteRequestDTO);
	}
}
