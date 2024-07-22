using Microsoft.AspNetCore.Mvc;

namespace Northwind.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet("/Account/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        [HttpGet("/Account/AccessDenied/{returnUrl}")]
        public IActionResult AccessDenied(string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            return RedirectToAction("AccessDenied");
        }
    }
}