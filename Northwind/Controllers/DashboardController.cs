using Dapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.Context;
using Northwind.Models;
using Northwind.Services.SQL;
using Northwind.ViewModels;

namespace Northwind.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public DashboardController(
	        ApplicationDbContext context, 
	        ISqlConnectionFactory sqlConnectionFactory)
        {
	        _context = context;
	        _sqlConnectionFactory = sqlConnectionFactory;
        }

        private const int PageSize = 10;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Products(int page = 1, string searchString = "")
        {
			using var connection = _sqlConnectionFactory.CreateConnection();

			var productsQuery = "SELECT * FROM Products";

			if (!string.IsNullOrEmpty(searchString))
			{
				productsQuery = """
				                SELECT * FROM 
				                Products WHERE ProductName 
				                LIKE @searchString
				                """;
			}

			var productsResult = connection.Query<Product>(productsQuery, new { searchString });

			var enumerable = productsResult.ToList();

			var totalProducts = enumerable.Count();

			var totalPages = (int)Math.Ceiling(totalProducts / (double)PageSize);

			var products = enumerable
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