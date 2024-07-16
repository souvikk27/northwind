using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Northwind.ViewModels;

namespace Northwind.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ResourcesController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
            RoleManager<IdentityRole> roleManager)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Scopes(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var excludedControllers = new HashSet<string> { "Auth" };

            var controllerInfos = _actionDescriptorCollectionProvider.ActionDescriptors.Items
                .Where(ad => ad.RouteValues["controller"] != null &&
                             !excludedControllers.Contains(ad.RouteValues["controller"]!))
                .GroupBy(ad => ad.RouteValues["controller"])
                .Select(g => new ControllerVm
                {
                    RoleId = id,
                    Name = g.Key,
                    ActionMethods = g.Select(ad => ad.RouteValues["action"])
                        .Distinct()
                        .Select(action => new ActionInfoVm
                        {
                            ActionMethodName = action,
                            IsAllowed = _roleManager.GetClaimsAsync(role)
                                .Result
                                .Any(c => c.Type == "Permission" && c.Value == $"{g.Key}-{action}")
                        })
                        .ToList()
                })
                .ToList();

            return View(controllerInfos);
        }

        [HttpPost]
        public async Task<IActionResult> SaveRolePermissions(List<RolePermissionUpdateVm> permissions)
        {
            if (permissions == null || !permissions.Any())
            {
                return BadRequest("No permissions provided.");
            }

            foreach (var permission in permissions)
            {
                var role = await _roleManager.FindByIdAsync(permission.RoleId);
                if (role == null)
                {
                    return NotFound($"Role with ID {permission.RoleId} not found.");
                }

                var claims = await _roleManager.GetClaimsAsync(role);

                foreach (var action in permission.Actions)
                {
                    var claimValue = $"{permission.ControllerName}-{action.ActionMethodName}";
                    var claim = claims.FirstOrDefault(c => c.Type == "Permission" && c.Value == claimValue);

                    if (action.IsAllowed)
                    {
                        if (claim == null)
                        {
                            await _roleManager.AddClaimAsync(role,
                                new Claim("Permission", claimValue));
                        }
                    }
                    else
                    {
                        if (claim != null)
                        {
                            await _roleManager.RemoveClaimAsync(role, claim);
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Roles"); // Adjust the redirect as needed
        }
    }
}