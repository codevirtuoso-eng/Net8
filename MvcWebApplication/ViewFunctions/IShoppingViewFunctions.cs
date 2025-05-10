
﻿using Microsoft.AspNetCore.Http;
using MvcWebApplication.ViewModels.Shopping;
using System.Threading.Tasks;

namespace MvcWebApplication.ViewFunctions
{
    public interface IShoppingViewFunctions
    {
        Task ProcessIndexRequest(IndexViewModel indexViewModel, HttpContext httpContext);
        Task ProcessAddToCartRequest(int itemId, int quantity, HttpContext httpContext);
    }
}
