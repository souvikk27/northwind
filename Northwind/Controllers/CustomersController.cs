using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Context;
using Northwind.ViewModels;

namespace Northwind.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            var totalCustomers = _context.Customers.Count();
            var totalPages = (int)Math.Ceiling(totalCustomers / (double)PageSize);

            var customers = _context.Customers
                .AsNoTracking()
                .OrderBy(c => c.ContactName)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var viewModel = new CustomerViewModel
            {
                Customers = customers,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalCustomers = totalCustomers
            };
            return View(viewModel);
        }
    }
}