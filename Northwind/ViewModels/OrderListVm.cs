using System;

namespace Northwind.ViewModels
{
    public class OrderListVm
    {
        public int OrderID { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string CustomerName { get; set; }
        public string EmployeeName { get; set; }
        public string Status => ShippedDate.HasValue ? "Shipped" : (RequiredDate.HasValue && RequiredDate.Value < DateTime.Now ? "Delayed" : "Pending");
    }
}
