using Microsoft.AspNetCore.Http;
using MvcWebApplication.ViewModels.Shopping;
using System.Threading.Tasks;

namespace MvcWebApplication.ViewFunctions
{
    public interface IShoppingViewFunctions
    {
        Task<IndexViewModel> ProcessIndexRequest(IndexViewModel model, HttpContext context);
        Task<bool> ProcessAddToCartRequest(int menuItemId, int quantity, HttpContext context);
    }
}