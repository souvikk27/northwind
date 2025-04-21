using System.Collections.Generic;

namespace Northwind.ViewModels
{
    public class EmployeesViewModel
    {
        public List<EmployeeListVm> Employees { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalEmployees { get; set; }
        public string SearchString { get; set; }
    }
}
