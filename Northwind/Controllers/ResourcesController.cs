using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Northwind.Services.Railway.Resource;
using Northwind.ViewModels;

namespace Northwind.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IResourceService _resourceService;

        public ResourcesController(
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
            RoleManager<IdentityRole> roleManager,
            IResourceService resourceService
        )
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
            _roleManager = roleManager;
            _resourceService = resourceService;
        }

        [HttpGet]
        //[Authorize(Policy = "RoleResourcePolicy")]
        public async Task<IActionResult> Scopes(string id)
        {
            var result = await _resourceService.GetScopesResult(id);
            return result.IsSuccess ? View(result.Value) : BadRequest(result.Error);
        }

        [HttpPost]
        //[Authorize(policy: "Resources-SaveRolePermissions")]
        public async Task<IActionResult> SaveRolePermissions(
            List<RolePermissionUpdateVm> permissions
        )
        {
            var result = await _resourceService.ValidateAndProcessPermissions(permissions);
            return result.IsSuccess ? RedirectToAction("Index", "Roles") : BadRequest(result.Error);
        }
    }
}
