using Northwind.Models;

namespace Northwind.ViewModels;

public class ProductListVm
{
    public IEnumerable<Product>? Products { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalProducts { get; set; }
    public string? SearchString { get; set; }
}