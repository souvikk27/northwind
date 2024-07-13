using Microsoft.AspNetCore.Identity;

namespace Northwind.Services.Railway;

public interface IRoleService
{
    Task<Result<IdentityResult>> DeleteRole(string id);
    Task<Result<IdentityResult>> AddRole(IdentityRole role);
    Task<Result<IdentityResult>> UpdateRole(string id, IdentityRole updatedRole);
}