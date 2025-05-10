using MvcWebApplication.Models;
using SharedLibrary.Common.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace MvcWebApplication.ViewModels.Orders
{
    public class IndexViewModel : BaseViewModel
    {
        public IndexViewModel()
        {
            OrderList = new List<Order>(); // advisable to ensure list is always initialized
            OrderSearch = new OrderSearch();
        }

        public List<Order> OrderList { get; set; }

        public OrderSearch OrderSearch { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}
