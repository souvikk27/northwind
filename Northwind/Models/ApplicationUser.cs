#nullable disable
using Microsoft.AspNetCore.Identity;

namespace Northwind.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}