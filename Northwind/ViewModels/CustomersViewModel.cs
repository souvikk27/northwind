using System.Collections.Generic;

namespace Northwind.ViewModels
{
    public class CustomersViewModel
    {
        public List<CustomerListVm> Customers { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCustomers { get; set; }
        public string SearchString { get; set; }
    }
}
