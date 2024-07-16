using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.Railway.Roles;
using Northwind.ViewModels;

namespace Northwind.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoleService _roleService;
        private const int PageSize = 5;

        public RolesController(RoleManager<IdentityRole> roleManager, IRoleService roleService)
        {
            _roleManager = roleManager;
            _roleService = roleService;
        }

        [HttpGet]
        public IActionResult Index(int page = 1, string searchString = "")
        {
            var rolesQuery = _roleManager.Roles.AsNoTracking();

            if (!string.IsNullOrEmpty(searchString))
            {
                rolesQuery = rolesQuery.Where(r => r.Name!.Contains(searchString));
            }

            var totalRoles = rolesQuery.Count();
            var totalPages = (int)Math.Ceiling(totalRoles / (double)PageSize);

            var roles = rolesQuery
                .OrderBy(r => r.Name)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var viewModel = new RolesVm
            {
                Roles = roles,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalRoles = totalRoles,
                SearchString = searchString
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string rolename)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _roleService.AddRole(new IdentityRole(rolename));
            return result.IsSuccess
                ? RedirectToActionWithSuccess("Role created successfully.")
                : RedirectToActionWithError(result.Error);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] IdentityRole role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }

            var result = await _roleService.UpdateRole(id, role);
            return result.IsSuccess
                ? RedirectToActionWithSuccess("Role updated successfully.")
                : HandleUpdateFailure(result.Error, role);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _roleService.DeleteRole(id);
            return result.IsSuccess
                ? RedirectToActionWithSuccess("Role deleted successfully.")
                : RedirectToActionWithError(result.Error);
        }

        [HttpGet]
        public IActionResult Manage(string id)
        {
            return RedirectToAction("Scopes", "Resources", new { id = id });
        }

        private IActionResult RedirectToActionWithSuccess(string message)
        {
            TempData["SuccessMessage"] = message;
            return RedirectToAction(nameof(Index));
        }

        private IActionResult RedirectToActionWithError(string error)
        {
            TempData["ErrorMessage"] = error;
            return RedirectToAction(nameof(Index));
        }

        private IActionResult HandleUpdateFailure(string error, IdentityRole role)
        {
            ModelState.AddModelError("", error);
            TempData["ErrorMessage"] = "There was an error updating the role. Please check the form and try again.";
            return View(role);
        }
    }
}