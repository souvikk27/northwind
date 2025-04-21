using System;

namespace Northwind.ViewModels
{
    public class EmployeeListVm
    {
        public int EmployeeID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Title { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public DateTime HireDate { get; set; }
        public int? ReportsTo { get; set; }
        public int OrderCount { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        public int YearsWithCompany =>
            DateTime.Now.Year
            - HireDate.Year
            - (DateTime.Now.DayOfYear < HireDate.DayOfYear ? 1 : 0);
    }
}
