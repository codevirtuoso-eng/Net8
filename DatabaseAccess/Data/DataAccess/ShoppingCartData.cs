using DatabaseAccess.Data.Context;
using DatabaseAccess.Data.EntityModels;
using DatabaseAccess.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedLibrary.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Data.DataAccess
{
	public class ShoppingCartData : IShoppingCartData
	{
		private readonly MainAppDbContext _mainAppDbContext;
		private readonly ILogger<ShoppingCartData> _logger;

		public ShoppingCartData(MainAppDbContext mainAppDbContext, ILogger<ShoppingCartData> logger)
		{
			_mainAppDbContext = mainAppDbContext;
			_logger = logger;
			_logger.LogDebug("NLog injected into ShoppingCartData");
		}

		public Task<List<ShoppingCartDAO>> GetShoppingCart(ShoppingCartSearch shoppingCartSearch)
		{
			throw new NotImplementedException();
		}
	}
}
