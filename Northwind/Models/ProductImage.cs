using System.ComponentModel.DataAnnotations;

namespace Northwind.Models;

public class ProductImage
{
    [Key]
    public int Id { get; set; }
    public string? Path { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
}