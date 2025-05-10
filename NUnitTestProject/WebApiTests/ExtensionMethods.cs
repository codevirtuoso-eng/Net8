using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitTestProject.WebApiTests
{

	// Making it internal since it is only to be used with unit tests
	internal static class ExtensionMethods
	{
		// Extension method for HttpContent to allow unit test for GetTokenAsync
		public static Task<string> GetTokenAsync(this HttpContext context, string scheme, string tokenName)
		{
			return context.RequestServices.GetRequiredService<IAuthenticationService>().GetTokenAsync(context, scheme, tokenName);
		}

		// Extension method for IAuthenticationService to allow unit test for GetTokenAsync
		public static async Task<string> GetTokenAsync(this IAuthenticationService auth, HttpContext context, string scheme, string tokenName)
		{
			if (auth == null)
			{
				throw new ArgumentNullException(nameof(auth));
			}
			if (tokenName == null)
			{
				throw new ArgumentNullException(nameof(tokenName));
			}
			var result = await auth.AuthenticateAsync(context, scheme);
			return result?.Properties?.GetTokenValue(tokenName);
		}
	}

}
