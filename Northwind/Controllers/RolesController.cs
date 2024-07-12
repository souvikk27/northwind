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
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(rolename);
        }
    }
}