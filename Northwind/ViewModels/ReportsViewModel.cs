using System.Collections.Generic;

namespace Northwind.ViewModels
{
    public class ReportsViewModel
    {
        public List<CategorySalesVm> SalesByCategory { get; set; }
        public List<CountrySalesVm> SalesByCountry { get; set; }
        public List<ProductSalesVm> TopProducts { get; set; }
        public List<MonthlySalesVm> MonthlySales { get; set; }
    }
}
