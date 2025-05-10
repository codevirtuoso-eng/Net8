
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedLibrary.DTO.ShoppingCart;
using System;
using System.Threading.Tasks;
using WebAppAPI.ApiFunctions;

namespace WebAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private IShoppingCartFunctions _shoppingCartFunctions;
        private readonly ILogger<ShoppingCartsController> _logger;

        public ShoppingCartsController(IShoppingCartFunctions shoppingCartFunctions, ILogger<ShoppingCartsController> logger)
        {
            _shoppingCartFunctions = shoppingCartFunctions;
            _logger = logger;
            _logger.LogDebug("NLog injected into ShoppingCartsController");
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("GetShoppingCart")]
        public async Task<ActionResult> GetShoppingCart(ShoppingCartSearchRequestDTO shoppingCartSearchRequestDTO)
        {
            _logger.LogInformation($"GetShoppingCart was called with shoppingCartSearchRequestDTO: {shoppingCartSearchRequestDTO}");
            
            try
            {
                var result = await _shoppingCartFunctions.GetShoppingCart(shoppingCartSearchRequestDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred getting shopping cart.");
                var responseObject = new { responseText = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, responseObject);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("CreateShoppingCart")]
        public async Task<ActionResult> CreateShoppingCart(ShoppingCartCreateRequestDTO shoppingCartCreateRequestDTO)
        {
            _logger.LogInformation($"CreateShoppingCart was called with shoppingCartCreateRequestDTO: {shoppingCartCreateRequestDTO}");
            
            try
            {
                await _shoppingCartFunctions.AddToCart(shoppingCartCreateRequestDTO);
                return Ok(new { responseText = "Item added to cart successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred creating shopping cart item.");
                var responseObject = new { responseText = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, responseObject);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("RemoveShoppingCartItem")]
        public async Task<ActionResult> RemoveShoppingCartItem(ShoppingCartRemoveRequestDTO shoppingCartRemoveRequestDTO)
        {
            _logger.LogInformation($"RemoveShoppingCartItem was called with shoppingCartRemoveRequestDTO: {shoppingCartRemoveRequestDTO}");
            
            try
            {
                await _shoppingCartFunctions.RemoveFromCart(shoppingCartRemoveRequestDTO);
                return Ok(new { responseText = "Item removed from cart successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred removing shopping cart item.");
                var responseObject = new { responseText = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, responseObject);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("EmptyShoppingCart")]
        public async Task<ActionResult> EmptyShoppingCart(ShoppingCartEmptyRequestDTO shoppingCartEmptyRequestDTO)
        {
            _logger.LogInformation($"EmptyShoppingCart was called with shoppingCartEmptyRequestDTO: {shoppingCartEmptyRequestDTO}");
            
            try
            {
                await _shoppingCartFunctions.EmptyCart(shoppingCartEmptyRequestDTO);
                return Ok(new { responseText = "Shopping cart emptied successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred emptying shopping cart.");
                var responseObject = new { responseText = ex.Message };
                return StatusCode(StatusCodes.Status500InternalServerError, responseObject);
            }
        }
    }
}
