using Microsoft.AspNetCore.Mvc;

namespace Northwind.Controllers
{
    public class RolesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
