using Microsoft.AspNetCore.Http;
using MvcWebApplication.ViewModels.MenuListings;
using System.Threading.Tasks;

namespace MvcWebApplication.ViewFunctions
{
    public interface IMenuListingViewFunctions
    {
        public Task ProcessIndexRequest(IndexViewModel indexViewModel, HttpContext httpContext);
        public Task ProcessSearchRequest(SearchViewModel searchViewModel, HttpContext httpContext);

    }
}
