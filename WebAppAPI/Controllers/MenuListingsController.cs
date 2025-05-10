using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedLibrary.DTO.MenuListing;
using SharedLibrary.Enums;
using System.Threading.Tasks;
using System;
using WebAppAPI.ApiFunctions;
using SharedLibrary.Common.Models;

namespace WebAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuListingsController : ControllerBase
    {
        private IMenuListingsFunctions _menuListingFunctions;
        private readonly ILogger<MenuListingsController> _logger;

        public MenuListingsController(IMenuListingsFunctions menuListingFunctions, ILogger<MenuListingsController> logger)
        {
            _menuListingFunctions = menuListingFunctions;
            _logger = logger;
            _logger.LogDebug("NLog injected into MenuListingController");
        }

        [Authorize(Roles = "User, Admin")] // prefer to use enums, but requires custom attribute
        [HttpPost]
        [Route("GetMenuListings")]
        public async Task<ActionResult> GetMenuListings(MenuListingSearchRequestDTO menuListingSearchRequestDTO)
        {
                _logger.LogInformation($"GetMenuListings was called with menuListingSearchRequestDTO: {menuListingSearchRequestDTO}");

            try
            {
                var result = await _menuListingFunctions.GetMenuListings(menuListingSearchRequestDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred getting menu listings.");
                var responseObject = new { responseText = ex.Message }; // Not necessarily a friendly message
                return StatusCode(StatusCodes.Status500InternalServerError, responseObject);
            }
        }

        [Authorize(Roles = "Admin")] // prefer to use enums, but requires custom attribute
        [HttpPost]
        [Route("CreateMenuListing")]
        public async Task<ActionResult> CreateMenuListing(MenuListingCreateRequestDTO menuListingCreateRequestDTO)
        {
            _logger.LogInformation($"CreateMenuListing was called with menuListingCreateRequestDTO: {menuListingCreateRequestDTO}");

            try
            {
                var menuListingCreateResponseDTO = await _menuListingFunctions.CreateMenuListing(menuListingCreateRequestDTO);
                return Ok(menuListingCreateResponseDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred creating menu listing.");
                var responseObject = new { responseText = ex.Message }; // Not necessarily a friendly message
                return StatusCode(StatusCodes.Status500InternalServerError, responseObject);
            }
        }

        [Authorize(Roles = "Admin")] // prefer to use enums, but requires custom attribute
        [HttpPost]
        [Route("GetMenuListing")]
        public async Task<ActionResult> GetMenuListing(MenuListingGetRequestDTO menuListingGetRequestDTO)
        {
            _logger.LogInformation($"GetMenuListing was called with menuListingGetRequestDTO: {menuListingGetRequestDTO}");

            try
            {
                var result = await _menuListingFunctions.GetMenuListing(menuListingGetRequestDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred getting menu listing.");
                var responseObject = new { responseText = ex.Message }; // Not necessarily a friendly message
                return StatusCode(StatusCodes.Status500InternalServerError, responseObject);
            }
        }

        [Authorize(Roles = "Admin")] // prefer to use enums, but requires custom attribute
        [HttpPost]
        [Route("UpdateMenuListing")]
        public async Task<ActionResult> UpdateMenuListing(MenuListingUpdateRequestDTO menuListingUpdateRequestDTO)
        {
            _logger.LogInformation($"UpdateMenuListing was called with menuListingUpdateRequestDTO: {menuListingUpdateRequestDTO}");

            try
            {
                await _menuListingFunctions.UpdateMenuListing(menuListingUpdateRequestDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred updating menu listing.");
                var responseObject = new { responseText = ex.Message }; // Not necessarily a friendly message
                return StatusCode(StatusCodes.Status500InternalServerError, responseObject);
            }
        }

        [Authorize(Roles = "Admin")] // prefer to use enums, but requires custom attribute
        [HttpPost]
        [Route("DeleteMenuListing")]
        public async Task<ActionResult> DeleteMenuListing(MenuListingDeleteRequestDTO menuListingDeleteRequestDTO)
        {
            _logger.LogInformation($"DeleteMenuListing was called with menuListingDeleteRequestDTO: {menuListingDeleteRequestDTO}");

            try
            {
                await _menuListingFunctions.DeleteMenuListing(menuListingDeleteRequestDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred deleting menu listing.");
                var responseObject = new { responseText = ex.Message }; // Not necessarily a friendly message
                return StatusCode(StatusCodes.Status500InternalServerError, responseObject);
            }
        }
    }
}
