using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.ViewModels;

namespace Northwind.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private const int PageSize = 5;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
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
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(rolename));
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Role created successfully!";
                    return View();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                TempData["ErrorMessage"] = "Failed to create role. Please check the errors.";
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid input. Please check your entries.";
            }

            return View(rolename);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] IdentityRole role)
        {
            if (id != role.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingRole = await _roleManager.FindByIdAsync(id);
                    if (existingRole == null)
                    {
                        return NotFound();
                    }

                    existingRole.Name = role.Name;
                    var result = await _roleManager.UpdateAsync(existingRole);

                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "Role updated successfully.";
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await RoleExists(role.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            TempData["ErrorMessage"] = "There was an error updating the role. Please check the form and try again.";
            return View(role);
        }

        private async Task<bool> RoleExists(string id)
        {
            return await _roleManager.RoleExistsAsync(id);
        }
    }
}