using Bogus;
using Northwind.Models;

namespace Northwind.Services.Seeding;

public class CategorySeeder
{
    public static List<Category> GenerateCategories(int count)
    {
        var categoryFaker = new Faker<Category>()
            .RuleFor(c => c.CategoryId, f => f.IndexFaker + 1)
            .RuleFor(c => c.CategoryName, f => f.Commerce.Categories(1)[0])
            .RuleFor(c => c.Description, f => f.Lorem.Sentence(10, 2))
            .RuleFor(c => c.Picture, f => f.Random.Bytes(10))
            .RuleFor(c => c.CreatedBy, f => Guid.NewGuid())
            .RuleFor(c => c.CreatedOn, f => f.Date.Past(2))
            .RuleFor(c => c.LastModifiedBy, f => f.Random.Bool(0.3f) ? Guid.NewGuid() : (Guid?)null)
            .RuleFor(c => c.LastModifiedOn,
                (f, c) => c.LastModifiedBy.HasValue ? f.Date.Between(c.CreatedOn, DateTime.UtcNow) : (DateTime?)null);
        return categoryFaker.Generate(count);
    }
}