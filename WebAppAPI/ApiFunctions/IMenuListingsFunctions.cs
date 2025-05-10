
using SharedLibrary.DTO.MenuListing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAppAPI.ApiFunctions
{
	public interface IMenuListingsFunctions
	{
		Task<List<MenuListingGetResponseDTO>> GetMenuListings(MenuListingSearchRequestDTO menuListingSearchRequestDTO);
		Task<MenuListingGetResponseDTO> GetMenuListing(MenuListingGetRequestDTO menuListingGetRequestDTO);
		Task<MenuListingCreateResponseDTO> CreateMenuListing(MenuListingCreateRequestDTO menuListingCreateRequestDTO);
		Task UpdateMenuListing(MenuListingUpdateRequestDTO menuListingUpdateRequestDTO);
		Task DeleteMenuListing(MenuListingDeleteRequestDTO menuListingDeleteRequestDTO);
	}
}
