using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace MvcWebApplication.Controllers
{
	[AllowAnonymous]
	public class ErrorController : Controller
	{

		private readonly ILogger<ErrorController> _logger;

		public ErrorController(ILogger<ErrorController> logger)
		{
			_logger = logger;
			_logger.LogDebug(1, "NLog injected into ErrorController");
		}

		public IActionResult Unhandled()
		{
			var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();

			_logger.LogError($"An unhandled error occurred.");

			var httpStatusCode = HttpContext.Response.StatusCode;
			var message = exception.Error.Message;
			var stackTrace = exception.Error.StackTrace;
			var identifier = HttpContext.TraceIdentifier;

			_logger.LogError($"httpStatusCode: {httpStatusCode} errorMessage: {message}");
			_logger.LogError($"stackTrace: {stackTrace}");

			return View();
		}


		public IActionResult Status(int? statusCode = null)
		{
			var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
			var originalPath = statusCodeReExecuteFeature.OriginalPath;
			var originalPathBase = statusCodeReExecuteFeature.OriginalPathBase;
			var originalQueryString = statusCodeReExecuteFeature.OriginalQueryString;

			_logger.LogError($"An error occurred with status code {statusCode}. Original base path {originalPathBase} Original path {originalPath} Original query string {originalQueryString}");

			switch (statusCode)
			{
				case 401:
					return RedirectToAction("Error401");

				case 404:
					return RedirectToAction("Error404");

				default:
					// TODO how to pass status code to error page along with base path, path, and query string??
					return RedirectToAction("ErrorStatusGeneral"); 

			}

			//return View(); // we really do not want to return a default view; instead reroute to another end point
		}

		public IActionResult Error401()
		{
			return View();
		}

		public IActionResult Error404()
		{
			return View();
		}
		
		public IActionResult ErrorStatusGeneral()
		{
			return View();
		}
	}
}
