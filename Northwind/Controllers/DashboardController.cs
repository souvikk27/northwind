using Microsoft.AspNetCore.Mvc;

namespace Northwind.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
