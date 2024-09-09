using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Context;
using Northwind.Models;
using Northwind.Services.SQL;
using Northwind.ViewModels;

namespace Northwind.Controllers
{
	public class ProductsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly ISqlConnectionFactory _sqlConnectionFactory;
		private const int PageSize = 5;

		public ProductsController(
			ApplicationDbContext context,
			ISqlConnectionFactory sqlConnectionFactory)
		{
			_context = context;
			_sqlConnectionFactory = sqlConnectionFactory;
		}

		public IActionResult Index(int page = 1, string searchString = "")
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

			var productsResult = connection.Query<Product>( productsQuery, new { searchString });

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
	}
}