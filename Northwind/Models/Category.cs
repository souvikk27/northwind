using System.ComponentModel.DataAnnotations;
using Northwind.Models.Abstractions;

namespace Northwind.Models;

public sealed class Category : AuditableBaseEntity, IEquatable<Category>, IValidatableObject
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public byte[]? Picture { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();

    public bool Equals(Category? other)
    {
        if (other is null)
        {
            return false;
        }

        return CategoryId == other.CategoryId &&
               CategoryName == other.CategoryName &&
               Description == other.Description &&
               Picture == other.Picture;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CategoryName.Length > 20)
        {
            yield return new ValidationResult("Category name cannot exceed 20 characters", new[] { "CategoryName" });
        }
        else if (CategoryName.Length < 1)
        {
            yield return new ValidationResult("Category name must not be null or contain less than 1 characters",
                new[] { "CategoryName" });
        }
        else if (Description is null)
        {
            yield return new ValidationResult("Category description must not be null", new[] { "Description" });
        }
        else if (Description.Length > 255)
        {
            yield return new ValidationResult("Category description cannot exceed 50 characters",
                new[] { "Description" });
        }
    }
}