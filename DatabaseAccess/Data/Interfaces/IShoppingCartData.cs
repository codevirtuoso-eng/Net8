using DatabaseAccess.Data.EntityModels;
using SharedLibrary.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Data.Interfaces
{
	public interface IShoppingCartData
	{
		public Task<List<ShoppingCartDAO>> GetShoppingCart(ShoppingCartSearch shoppingCartSearch);
	}
}
