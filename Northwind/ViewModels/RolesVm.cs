using Microsoft.AspNetCore.Identity;
#nullable disable
namespace Northwind.ViewModels
{
    public class RolesVm
    {
        public IEnumerable<IdentityRole> Roles { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRoles { get; set; }
        public string SearchString { get; set; }
    }
}
