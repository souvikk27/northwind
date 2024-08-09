using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Context;
using Northwind.ViewModels;

namespace Northwind.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 5;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1, string searchString = "")
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
    }
}