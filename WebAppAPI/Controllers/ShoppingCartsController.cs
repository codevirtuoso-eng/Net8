using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Enums;
using System.Threading.Tasks;
using System;

namespace WebAppAPI.Controllers
{
	public class ShoppingCartsController : Controller
	{
		[HttpPost]
		[Route("GetShoppingCart")]
		public async Task<ActionResult> GetShoppingCart()
		{
			return Ok();
		}

		[HttpPost]
		[Route("CreateShoppingCart")]
		public async Task<ActionResult> CreateShoppingCart()
		{
			return Ok();
		}

		[HttpPost]
		[Route("RemoveShoppingCartItem")]
		public async Task<ActionResult> RemoveShoppingCartItem()
		{
			return Ok();

		}

		[Authorize(Roles = "User")] // prefer to use enums, but requires custom attribute
		[HttpPost]
		[Route("EmptyShoppingCart")]
		public async Task<ActionResult> EmptyShoppingCart()
		{
			return Ok();
		}
	}
}
