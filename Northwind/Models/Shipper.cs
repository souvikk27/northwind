using Northwind.Models.Abstractions;

namespace Northwind.Models;

public partial class Shipper : AuditableBaseEntity
{
    public int ShipperId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
