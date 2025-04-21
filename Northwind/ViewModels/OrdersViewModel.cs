using System.Collections.Generic;

namespace Northwind.ViewModels
{
    public class OrdersViewModel
    {
        public List<OrderListVm> Orders { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalOrders { get; set; }
        public string SearchString { get; set; }
    }
}
