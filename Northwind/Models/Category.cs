using System.ComponentModel.DataAnnotations;

namespace Northwind.Models;

public partial class Category : IEquatable<Category>, IValidatableObject
{
	public int CategoryId { get; set; }

	public string CategoryName { get; set; } = null!;

	public string? Description { get; set; }

	public byte[]? Picture { get; set; }

	public virtual ICollection<Product> Products { get; set; } = new List<Product>();

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
			yield return new ValidationResult("Category name must be at least 1 character", new[] { "CategoryName" });
		}
		else if (CategoryName.Contains(" "))
		{
			yield return new ValidationResult("Category name cannot contain spaces", new[] { "CategoryName" });
		}

	}
}
