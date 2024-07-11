using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Northwind.Controllers
{
    public class RolesController : Controller
    {
	    private readonly RoleManager<IdentityRole> _roleManager;

	    public RolesController(RoleManager<IdentityRole> roleManager)
	    {
		    _roleManager = roleManager;
	    }

		[HttpGet]
	    public IActionResult Index()
        {
            return View();
        }

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(string name)
		{
			if (ModelState.IsValid)
			{
				IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
				if (result.Succeeded)
				{
					return RedirectToAction("Index");
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
			return View(name);
		}	
    }
}
