using Microsoft.AspNetCore.Mvc;

namespace Northwind.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}