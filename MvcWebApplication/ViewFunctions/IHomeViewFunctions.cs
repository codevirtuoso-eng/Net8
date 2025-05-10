using Microsoft.AspNetCore.Http;
using MvcWebApplication.ViewModels.Home;
using MvcWebApplication.ViewModels.Orders;
using SharedLibrary.DTO.AspNetUser;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvcWebApplication.ViewFunctions
{
	public interface IHomeViewFunctions
	{
		public Task Login(HomeLoginViewModel homeLoginViewModel);

		public Task<List<AspNetUserResponseDTO>> GetAllUsers(HttpContext httpContext);
	}
}
