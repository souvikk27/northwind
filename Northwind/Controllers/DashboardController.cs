using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Context;
using Northwind.ViewModels;

namespace Northwind.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        private const int PageSize = 10;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Products(int page = 1, string searchString = "")
        {
            var productsQuery = _context.Products.AsNoTracking();
            if (!string.IsNullOrEmpty(searchString))
            {
                productsQuery = productsQuery.Where(p => p.ProductName.Contains(searchString));
            }

            var totalProducts = productsQuery.Count();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)PageSize);

            var products = productsQuery
                .OrderBy(p => p.ProductName)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var viewModel = new ProductListVm()
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalProducts = totalProducts,
                SearchString = searchString
            };
            return View(viewModel);
        }

        [HttpGet]
        [Route("Dashboard/Products/Add")]
        public IActionResult CreateProducts()
        {
            return View();
        }
    }
}