
﻿using Microsoft.AspNetCore.Http;
using MvcWebApplication.ViewModels.ShoppingCarts;
using System.Threading.Tasks;

namespace MvcWebApplication.ViewFunctions
{
    public interface IShoppingCartViewFunctions
    {
        Task ProcessIndexRequest(IndexViewModel indexViewModel, HttpContext httpContext);
        Task ProcessRemoveItemRequest(int itemId, HttpContext httpContext);
        Task ProcessEmptyCartRequest(HttpContext httpContext);
        Task ProcessCheckoutRequest(HttpContext httpContext);
    }
}
