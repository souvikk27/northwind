using Northwind.Models;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Northwind.ViewModels;

public class CustomerViewModel
{
    public IEnumerable<Customer> Customers { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalCustomers { get; set; }
}