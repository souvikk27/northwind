using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Northwind.Controllers
{
    public class UsersController : Controller
    {
        [HttpGet, Authorize(policy: "Users-Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}