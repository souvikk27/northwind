using Microsoft.AspNetCore.Identity;
using Northwind.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Northwind.Services.Railway.Resource
{
    public class ResourceService : IResourceService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public ResourceService(RoleManager<IdentityRole> roleManager,
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _roleManager = roleManager;
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        public async Task<Result<List<ControllerVm>>> GetScopesResult(string id)
        {
            return await FindRole(id)
                .Result.BindAsync(async role =>
                {
                    var claims = await _roleManager.GetClaimsAsync(role);
                    return GetControllerInfos(role, claims);
                });
        }

        private Result<List<ControllerVm>> GetControllerInfos(IdentityRole role, IEnumerable<Claim> claims)
        {
            var excludedControllers = new HashSet<string> { "Auth" };

            var controllerInfos = _actionDescriptorCollectionProvider.ActionDescriptors.Items
                .Where(ad => ad.RouteValues["controller"] != null &&
                             !excludedControllers.Contains(ad.RouteValues["controller"]!))
                .GroupBy(ad => ad.RouteValues["controller"])
                .Select(g => new ControllerVm
                {
                    RoleId = role.Id,
                    Name = g.Key,
                    ActionMethods = g.Select(ad => ad.RouteValues["action"])
                        .Distinct()
                        .Select(action => new ActionInfoVm
                        {
                            ActionMethodName = action,
                            IsAllowed = claims.Any(c => c.Type == "Permission" && c.Value == $"{g.Key}-{action}")
                        })
                        .ToList()
                })
                .ToList();

            return Result<List<ControllerVm>>.Success(controllerInfos);
        }


        public async Task<Result<bool>> ValidateAndProcessPermissions(List<RolePermissionUpdateVm> permissions)
        {
            if (permissions == null || !permissions.Any())
            {
                return Result<bool>.Failure("No permissions provided.");
            }

            foreach (var permission in permissions)
            {
                var result = await ProcessPermission(permission);
                if (!result.IsSuccess)
                {
                    return Result<bool>.Failure($"Error processing permission: {result.Error}");
                }
            }

            return Result<bool>.Success(true);
        }

        private async Task<Result<bool>> ProcessPermission(RolePermissionUpdateVm permission)
        {
            var roleResult = await FindRole(permission.RoleId);
            if (!roleResult.IsSuccess)
            {
                return Result<bool>.Failure(roleResult.Error);
            }

            var role = roleResult.Value;

            foreach (var action in permission.Actions)
            {
                var claims = await _roleManager.GetClaimsAsync(role);
                var result = await ProcessAction(role, claims, permission.ControllerName, action);
                if (!result.IsSuccess)
                {
                    return result;
                }
            }
            return Result<bool>.Success(true);
        }

        private async Task<Result<IdentityRole>> FindRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            return role != null
                ? Result<IdentityRole>.Success(role)
                : Result<IdentityRole>.Failure($"Role with ID {roleId} not found.");
        }

        private async Task<Result<IList<Claim>>> GetRoleClaims(IdentityRole role)
        {
            var claims = await _roleManager.GetClaimsAsync(role);
            return Result<IList<Claim>>.Success(claims);
        }

        private async Task<Result<bool>> ProcessAction(IdentityRole role, IEnumerable<Claim> claims,
            string controllerName,
            ActionPermission action)
        {
            var claimValue = $"{controllerName}-{action.ActionMethodName}";
            var claim = claims.FirstOrDefault(c => c.Type == "Permission" && c.Value == claimValue);

            return action.IsAllowed switch
            {
                true when claim == null => await AddClaim(role, claimValue),
                false when claim != null => await RemoveClaim(role, claim),
                _ => Result<bool>.Success(true)
            };
        }

        private async Task<Result<bool>> AddClaim(IdentityRole role, string claimValue)
        {
            var result = await _roleManager.AddClaimAsync(role, new Claim("Permission", claimValue));
            return result.Succeeded
                ? Result<bool>.Success(true)
                : Result<bool>.Failure($"Failed to add claim {claimValue} to role {role.Name}");
        }

        private async Task<Result<bool>> RemoveClaim(IdentityRole role, Claim claim)
        {
            var result = await _roleManager.RemoveClaimAsync(role, claim);
            return result.Succeeded
                ? Result<bool>.Success(true)
                : Result<bool>.Failure($"Failed to remove claim {claim.Value} from role {role.Name}");
        }

        #region Unused Methods might be used later?
        private async Task<Result<bool>> ProcessActions(IdentityRole role, IList<Claim> claims, string controllerName,
            IEnumerable<ActionPermission> actions)
        {
            var results =
                await Task.WhenAll(actions.Select(action => ProcessAction(role, claims, controllerName, action)));

            if (results.Any(r => !r.IsSuccess))
            {
                var errors = string.Join(", ", results.Where(r => !r.IsSuccess).Select(r => r.Error));
                return Result<bool>.Failure($"Errors occurred while processing actions: {errors}");
            }

            return Result<bool>.Success(true);
        }
        #endregion
    }
}