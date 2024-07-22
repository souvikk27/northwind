using Microsoft.AspNetCore.Mvc;

namespace Northwind.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet("/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            return statusCode switch
            {
                403 => View("AccessDenied"),
                _ => View("Error")
            };
        }
    }
}