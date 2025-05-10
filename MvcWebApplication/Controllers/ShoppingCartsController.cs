using Microsoft.AspNetCore.Mvc;

namespace MvcWebApplication.Controllers
{
	public class ShoppingCartsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
