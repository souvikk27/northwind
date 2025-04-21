using System.Collections.Generic;

namespace Northwind.ViewModels
{
    public class SuppliersViewModel
    {
        public List<SupplierListVm> Suppliers { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalSuppliers { get; set; }
        public string SearchString { get; set; }
    }
}
