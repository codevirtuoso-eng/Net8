
﻿using MvcWebApplication.Models;
using System;
using System.Collections.Generic;

namespace MvcWebApplication.ViewModels.Orders
{
    public class OrdersGetOrderDetailsViewModel : BaseViewModel
    {
        public OrdersGetOrderDetailsViewModel()
        {
            OrderDetails = new List<OrderDetail>();
        }

        public string OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
