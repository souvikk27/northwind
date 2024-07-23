using Microsoft.AspNetCore.Identity;

namespace Northwind.Services.Railway.Roles;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    private async Task<Result<IdentityRole>> FindRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        return role != null
            ? Result<IdentityRole>.Success(role)
            : Result<IdentityRole>.Failure("Role not found");
    }

    private Result<(string id, IdentityRole role)> ValidateInput(string id, IdentityRole role)
    {
        return id == role.Id
            ? Result<(string, IdentityRole)>.Success((id, role))
            : Result<(string, IdentityRole)>.Failure("Invalid input");
    }

    private async Task<Result<IdentityRole>> CheckRoleExists(string name)
    {
        var role = await _roleManager.FindByNameAsync(name);
        return role != null
            ? Result<IdentityRole>.Failure("Role with the same name already exists")
            : Result<IdentityRole>.Success(default!);
    }

    public async Task<Result<IdentityResult>> AddRole(IdentityRole role)
    {
        return await CheckRoleExists(role.Name!)
            .Result.BindAsync(async _ =>
            {
                var result = await _roleManager.CreateAsync(role);
                return result.Succeeded
                    ? Result<IdentityResult>.Success(result)
                    : Result<IdentityResult>.Failure("Failed to create role");
            });
    }

    public async Task<Result<IdentityResult>> UpdateRole(string id, IdentityRole updatedRole)
    {
        return await ValidateInput(id, updatedRole)
            .BindAsync(async _ => await FindRole(id))
            .Result.BindAsync(async existingRole =>
            {
                existingRole.Name = updatedRole.Name;
                var result = await _roleManager.UpdateAsync(existingRole);
                return result.Succeeded
                    ? Result<IdentityResult>.Success(result)
                    : Result<IdentityResult>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));
            });
    }
    public async Task<Result<IdentityResult>> DeleteRole(string id)
    {
        return await FindRole(id)
            .Result.BindAsync(async role =>
            {
                var result = await _roleManager.DeleteAsync(role);
                return result.Succeeded
                    ? Result<IdentityResult>.Success(result)
                    : Result<IdentityResult>.Failure("Failed to delete role");
            });
    }
}