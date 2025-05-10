using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Enums;

namespace MvcWebApplication.Controllers
{

	public class ShoppingController : Controller
	{
		[Authorize(Roles = "User")]
		public IActionResult Index()
		{
			return View();
		}
	}
}
