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

        [HttpGet]
        public IActionResult Orders(int page = 1, string searchString = "")
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            var ordersQuery = "SELECT o.OrderID, o.OrderDate, o.RequiredDate, o.ShippedDate, "
                            + "c.CompanyName as CustomerName, e.FirstName + ' ' + e.LastName as EmployeeName "
                            + "FROM Orders o "
                            + "JOIN Customers c ON o.CustomerID = c.CustomerID "
                            + "JOIN Employees e ON o.EmployeeID = e.EmployeeID";

            if (!string.IsNullOrEmpty(searchString))
            {
                ordersQuery += " WHERE c.CompanyName LIKE @searchString OR "
                             + "e.FirstName + ' ' + e.LastName LIKE @searchString";
            }

            var ordersResult = connection.Query<OrderListVm>(ordersQuery, new { searchString = $"%{searchString}%" });

            var enumerable = ordersResult.ToList();

            var totalOrders = enumerable.Count();

            var totalPages = (int)Math.Ceiling(totalOrders / (double)PageSize);

            var orders = enumerable
                .OrderByDescending(o => o.OrderDate)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var viewModel = new OrdersViewModel
            {
                Orders = orders,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalOrders = totalOrders,
                SearchString = searchString
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Customers(int page = 1, string searchString = "")
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            var customersQuery = "SELECT c.CustomerID, c.CompanyName, c.ContactName, c.ContactTitle, "
                               + "c.Country, c.Phone, COUNT(o.OrderID) as OrderCount "
                               + "FROM Customers c "
                               + "LEFT JOIN Orders o ON c.CustomerID = o.CustomerID";

            if (!string.IsNullOrEmpty(searchString))
            {
                customersQuery += " WHERE c.CompanyName LIKE @searchString OR "
                                + "c.ContactName LIKE @searchString OR "
                                + "c.Country LIKE @searchString";
            }

            customersQuery += " GROUP BY c.CustomerID, c.CompanyName, c.ContactName, c.ContactTitle, c.Country, c.Phone";

            var customersResult = connection.Query<CustomerListVm>(customersQuery, new { searchString = $"%{searchString}%" });

            var enumerable = customersResult.ToList();

            var totalCustomers = enumerable.Count();

            var totalPages = (int)Math.Ceiling(totalCustomers / (double)PageSize);

            var customers = enumerable
                .OrderBy(c => c.CompanyName)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var viewModel = new CustomersViewModel
            {
                Customers = customers,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalCustomers = totalCustomers,
                SearchString = searchString
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Suppliers(int page = 1, string searchString = "")
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            var suppliersQuery = "SELECT s.SupplierID, s.CompanyName, s.ContactName, s.ContactTitle, "
                               + "s.Country, s.Phone, COUNT(p.ProductID) as ProductCount "
                               + "FROM Suppliers s "
                               + "LEFT JOIN Products p ON s.SupplierID = p.SupplierID";

            if (!string.IsNullOrEmpty(searchString))
            {
                suppliersQuery += " WHERE s.CompanyName LIKE @searchString OR "
                                + "s.ContactName LIKE @searchString OR "
                                + "s.Country LIKE @searchString";
            }

            suppliersQuery += " GROUP BY s.SupplierID, s.CompanyName, s.ContactName, s.ContactTitle, s.Country, s.Phone";

            var suppliersResult = connection.Query<SupplierListVm>(suppliersQuery, new { searchString = $"%{searchString}%" });

            var enumerable = suppliersResult.ToList();

            var totalSuppliers = enumerable.Count();

            var totalPages = (int)Math.Ceiling(totalSuppliers / (double)PageSize);

            var suppliers = enumerable
                .OrderBy(s => s.CompanyName)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var viewModel = new SuppliersViewModel
            {
                Suppliers = suppliers,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalSuppliers = totalSuppliers,
                SearchString = searchString
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Employees(int page = 1, string searchString = "")
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            var employeesQuery = "SELECT e.EmployeeID, e.FirstName, e.LastName, e.Title, "
                               + "e.City, e.Country, e.HireDate, e.ReportsTo, "
                               + "COUNT(o.OrderID) as OrderCount "
                               + "FROM Employees e "
                               + "LEFT JOIN Orders o ON e.EmployeeID = o.EmployeeID";

            if (!string.IsNullOrEmpty(searchString))
            {
                employeesQuery += " WHERE e.FirstName LIKE @searchString OR "
                                + "e.LastName LIKE @searchString OR "
                                + "e.Title LIKE @searchString";
            }

            employeesQuery += " GROUP BY e.EmployeeID, e.FirstName, e.LastName, e.Title, e.City, e.Country, e.HireDate, e.ReportsTo";

            var employeesResult = connection.Query<EmployeeListVm>(employeesQuery, new { searchString = $"%{searchString}%" });

            var enumerable = employeesResult.ToList();

            var totalEmployees = enumerable.Count();

            var totalPages = (int)Math.Ceiling(totalEmployees / (double)PageSize);

            var employees = enumerable
                .OrderBy(e => e.LastName)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var viewModel = new EmployeesViewModel
            {
                Employees = employees,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalEmployees = totalEmployees,
                SearchString = searchString
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Reports()
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            // Sales by Category
            var salesByCategoryQuery = "SELECT c.CategoryName, SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)) as TotalSales "
                                     + "FROM [Order Details] od "
                                     + "JOIN Products p ON od.ProductID = p.ProductID "
                                     + "JOIN Categories c ON p.CategoryID = c.CategoryID "
                                     + "GROUP BY c.CategoryName "
                                     + "ORDER BY TotalSales DESC";

            var salesByCategory = connection.Query<CategorySalesVm>(salesByCategoryQuery).ToList();

            // Sales by Country
            var salesByCountryQuery = "SELECT c.Country, SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)) as TotalSales "
                                    + "FROM [Order Details] od "
                                    + "JOIN Orders o ON od.OrderID = o.OrderID "
                                    + "JOIN Customers c ON o.CustomerID = c.CustomerID "
                                    + "GROUP BY c.Country "
                                    + "ORDER BY TotalSales DESC";

            var salesByCountry = connection.Query<CountrySalesVm>(salesByCountryQuery).ToList();

            // Top Products
            var topProductsQuery = "SELECT TOP 5 p.ProductName, SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)) as TotalSales "
                                 + "FROM [Order Details] od "
                                 + "JOIN Products p ON od.ProductID = p.ProductID "
                                 + "GROUP BY p.ProductName "
                                 + "ORDER BY TotalSales DESC";

            var topProducts = connection.Query<ProductSalesVm>(topProductsQuery).ToList();

            // Monthly Sales
            var monthlySalesQuery = "SELECT MONTH(o.OrderDate) as Month, YEAR(o.OrderDate) as Year, "
                                  + "SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)) as TotalSales "
                                  + "FROM [Order Details] od "
                                  + "JOIN Orders o ON od.OrderID = o.OrderID "
                                  + "WHERE o.OrderDate IS NOT NULL "
                                  + "GROUP BY MONTH(o.OrderDate), YEAR(o.OrderDate) "
                                  + "ORDER BY Year, Month";

            var monthlySales = connection.Query<MonthlySalesVm>(monthlySalesQuery).ToList();

            var viewModel = new ReportsViewModel
            {
                SalesByCategory = salesByCategory,
                SalesByCountry = salesByCountry,
                TopProducts = topProducts,
                MonthlySales = monthlySales
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Settings()
        {
            var viewModel = new SettingsViewModel
            {
                CompanyName = "Northwind Traders",
                CompanyEmail = "info@northwind.com",
                CompanyPhone = "(123) 456-7890",
                CompanyAddress = "123 Main St, Seattle, WA 98101",
                Currency = "USD",
                DateFormat = "MM/dd/yyyy",
                TimeZone = "Pacific Standard Time",
                Language = "English",
                Theme = "Light",
                NotificationsEnabled = true,
                EmailNotifications = true,
                PushNotifications = false,
                TwoFactorAuth = true,
                AutomaticBackups = true,
                BackupFrequency = "Daily"
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var viewModel = new ProfileViewModel
            {
                UserId = 1,
                FirstName = "Andrew",
                LastName = "Fuller",
                Email = "andrew.fuller@northwind.com",
                JobTitle = "Vice President, Sales",
                Department = "Sales",
                Phone = "(206) 555-9482",
                Address = "908 W. Capital Way",
                City = "Tacoma",
                Region = "WA",
                PostalCode = "98401",
                Country = "USA",
                HireDate = new DateTime(2012, 8, 14),
                BirthDate = new DateTime(1982, 2, 19),
                ReportsTo = "Steven Buchanan",
                Notes = "Andrew received his BTS commercial in 1974 and a Ph.D. in international marketing from the University of Dallas in 1981. He is fluent in French and Italian and reads German. He joined the company as a sales representative, was promoted to sales manager in January 2016, and to vice president of sales in March 2018. Andrew is a member of the Sales Management Roundtable, the Seattle Chamber of Commerce, and the Pacific Rim Importers Association.",
                PhotoUrl = "/images/profile/andrew-fuller.jpg",
                TotalSales = 457820.75m,
                TotalOrders = 127,
                AverageOrderValue = 3604.89m,
                TopSellingCategory = "Beverages",
                LastLoginDate = DateTime.Now.AddDays(-1),
                AccountCreatedDate = new DateTime(2012, 8, 14),
                IsActive = true,
                IsAdmin = true,
                HasTwoFactorEnabled = true,
                PreferredLanguage = "English",
                TimeZone = "Pacific Standard Time",
                NotificationsEnabled = true,
                EmailNotificationsEnabled = true,
                PushNotificationsEnabled = false,
                MarketingEmailsEnabled = true
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Earnings()
        {
            // Generate some sample monthly earnings data for the past 12 months
            var random = new Random(123); // Fixed seed for consistent results
            var monthlyEarnings = new List<MonthlyEarningVm>();
            var now = DateTime.Now;

            for (int i = 11; i >= 0; i--)
            {
                var month = now.AddMonths(-i);
                var baseAmount = 35000 + random.Next(-5000, 8000);
                var commissions = baseAmount * 0.15m;
                var bonuses = month.Month == 12 ? baseAmount * 0.2m : baseAmount * 0.05m; // Higher bonus in December
                var taxes = (baseAmount + commissions + bonuses) * 0.3m;
                var netEarnings = baseAmount + commissions + bonuses - taxes;

                monthlyEarnings.Add(new MonthlyEarningVm
                {
                    Month = month.Month,
                    Year = month.Year,
                    BaseAmount = baseAmount,
                    Commissions = commissions,
                    Bonuses = bonuses,
                    Taxes = taxes,
                    NetEarnings = netEarnings
                });
            }

            // Generate category earnings
            var categoryEarnings = new List<CategoryEarningVm>
            {
                new CategoryEarningVm { Category = "Beverages", Amount = 157820.75m },
                new CategoryEarningVm { Category = "Condiments", Amount = 85340.50m },
                new CategoryEarningVm { Category = "Confections", Amount = 98450.25m },
                new CategoryEarningVm { Category = "Dairy Products", Amount = 142780.30m },
                new CategoryEarningVm { Category = "Grains/Cereals", Amount = 75620.80m },
                new CategoryEarningVm { Category = "Meat/Poultry", Amount = 120450.60m },
                new CategoryEarningVm { Category = "Produce", Amount = 68940.25m },
                new CategoryEarningVm { Category = "Seafood", Amount = 95340.50m }
            };

            // Generate customer earnings
            var customerEarnings = new List<CustomerEarningVm>
            {
                new CustomerEarningVm { Customer = "QUICK-Stop", Amount = 95420.50m },
                new CustomerEarningVm { Customer = "Ernst Handel", Amount = 87650.75m },
                new CustomerEarningVm { Customer = "Save-a-lot Markets", Amount = 82340.25m },
                new CustomerEarningVm { Customer = "Rattlesnake Canyon Grocery", Amount = 78920.60m },
                new CustomerEarningVm { Customer = "Hungry Coyote Import Store", Amount = 65780.30m }
            };

            var viewModel = new EarningsViewModel
            {
                TotalEarnings = monthlyEarnings.Sum(e => e.NetEarnings),
                TotalCommissions = monthlyEarnings.Sum(e => e.Commissions),
                TotalBonuses = monthlyEarnings.Sum(e => e.Bonuses),
                TotalTaxes = monthlyEarnings.Sum(e => e.Taxes),
                MonthlyEarnings = monthlyEarnings,
                CategoryEarnings = categoryEarnings,
                CustomerEarnings = customerEarnings,
                CurrentMonthEarnings = monthlyEarnings.Last().NetEarnings,
                PreviousMonthEarnings = monthlyEarnings[monthlyEarnings.Count - 2].NetEarnings,
                YearToDateEarnings = monthlyEarnings.Sum(e => e.NetEarnings),
                ProjectedAnnualEarnings = monthlyEarnings.Sum(e => e.NetEarnings) * 12 / monthlyEarnings.Count
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Taxes()
        {
            // Generate some sample tax data
            var random = new Random(123); // Fixed seed for consistent results
            var monthlyTaxes = new List<MonthlyTaxVm>();
            var now = DateTime.Now;

            for (int i = 11; i >= 0; i--)
            {
                var month = now.AddMonths(-i);
                var income = 35000 + random.Next(-5000, 8000);
                var federalTax = income * 0.22m;
                var stateTax = income * 0.05m;
                var socialSecurity = income * 0.062m;
                var medicare = income * 0.0145m;
                var retirement401k = income * 0.05m;
                var healthInsurance = 350m;
                var otherDeductions = income * 0.01m;
                var totalTax = federalTax + stateTax + socialSecurity + medicare;
                var totalDeductions = retirement401k + healthInsurance + otherDeductions;
                var netIncome = income - totalTax - totalDeductions;

                monthlyTaxes.Add(new MonthlyTaxVm
                {
                    Month = month.Month,
                    Year = month.Year,
                    Income = income,
                    FederalTax = federalTax,
                    StateTax = stateTax,
                    SocialSecurity = socialSecurity,
                    Medicare = medicare,
                    Retirement401k = retirement401k,
                    HealthInsurance = healthInsurance,
                    OtherDeductions = otherDeductions,
                    TotalTax = totalTax,
                    TotalDeductions = totalDeductions,
                    NetIncome = netIncome
                });
            }

            // Generate tax documents
            var taxDocuments = new List<TaxDocumentVm>
            {
                new TaxDocumentVm { DocumentName = "W-2 Form", Year = now.Year - 1, DateIssued = new DateTime(now.Year, 1, 31), Status = "Available", DownloadUrl = "/documents/taxes/w2-2022.pdf" },
                new TaxDocumentVm { DocumentName = "1099-MISC", Year = now.Year - 1, DateIssued = new DateTime(now.Year, 1, 31), Status = "Available", DownloadUrl = "/documents/taxes/1099-misc-2022.pdf" },
                new TaxDocumentVm { DocumentName = "Tax Return", Year = now.Year - 1, DateIssued = new DateTime(now.Year, 4, 15), Status = "Filed", DownloadUrl = "/documents/taxes/tax-return-2022.pdf" },
                new TaxDocumentVm { DocumentName = "W-2 Form", Year = now.Year - 2, DateIssued = new DateTime(now.Year - 1, 1, 31), Status = "Archived", DownloadUrl = "/documents/taxes/w2-2021.pdf" },
                new TaxDocumentVm { DocumentName = "Tax Return", Year = now.Year - 2, DateIssued = new DateTime(now.Year - 1, 4, 15), Status = "Archived", DownloadUrl = "/documents/taxes/tax-return-2021.pdf" }
            };

            var viewModel = new TaxesViewModel
            {
                CurrentYear = now.Year,
                TotalIncome = monthlyTaxes.Sum(t => t.Income),
                TotalFederalTax = monthlyTaxes.Sum(t => t.FederalTax),
                TotalStateTax = monthlyTaxes.Sum(t => t.StateTax),
                TotalSocialSecurity = monthlyTaxes.Sum(t => t.SocialSecurity),
                TotalMedicare = monthlyTaxes.Sum(t => t.Medicare),
                TotalRetirement401k = monthlyTaxes.Sum(t => t.Retirement401k),
                TotalHealthInsurance = monthlyTaxes.Sum(t => t.HealthInsurance),
                TotalOtherDeductions = monthlyTaxes.Sum(t => t.OtherDeductions),
                EffectiveTaxRate = monthlyTaxes.Sum(t => t.TotalTax) / monthlyTaxes.Sum(t => t.Income) * 100,
                MonthlyTaxes = monthlyTaxes,
                TaxDocuments = taxDocuments,
                FederalFilingStatus = "Married Filing Jointly",
                Allowances = 2,
                EstimatedAnnualIncome = monthlyTaxes.Sum(t => t.Income) * 12 / monthlyTaxes.Count,
                EstimatedAnnualTax = monthlyTaxes.Sum(t => t.TotalTax) * 12 / monthlyTaxes.Count,
                YearToDateWithholding = monthlyTaxes.Sum(t => t.TotalTax)
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Refunds()
        {
            // Generate some sample refund data
            var random = new Random(123); // Fixed seed for consistent results
            var refundRequests = new List<RefundRequestVm>();
            var now = DateTime.Now;
            var statuses = new[] { "Pending", "Approved", "Processed", "Rejected" };
            var reasons = new[] { "Customer Dissatisfaction", "Product Defect", "Wrong Item", "Late Delivery", "Changed Mind" };

            for (int i = 0; i < 15; i++)
            {
                var createdDate = now.AddDays(-random.Next(1, 90));
                var amount = 50 + random.Next(10, 500);
                var status = statuses[random.Next(0, statuses.Length)];
                var processedDate = status == "Pending" ? (DateTime?)null : createdDate.AddDays(random.Next(1, 7));

                refundRequests.Add(new RefundRequestVm
                {
                    RefundId = 10000 + i,
                    OrderId = 20000 + random.Next(1, 1000),
                    CustomerName = $"Customer {random.Next(1, 100)}",
                    Amount = amount,
                    Reason = reasons[random.Next(0, reasons.Length)],
                    CreatedDate = createdDate,
                    ProcessedDate = processedDate,
                    Status = status,
                    Notes = status == "Rejected" ? "Customer policy violation" : ""
                });
            }

            // Generate refund statistics
            var totalRefunds = refundRequests.Count;
            var totalRefundAmount = refundRequests.Sum(r => r.Amount);
            var approvedRefunds = refundRequests.Count(r => r.Status == "Approved" || r.Status == "Processed");
            var pendingRefunds = refundRequests.Count(r => r.Status == "Pending");
            var rejectedRefunds = refundRequests.Count(r => r.Status == "Rejected");
            var averageProcessingTime = 3.5; // In days

            // Generate refund by reason data
            var refundsByReason = reasons.Select(reason => new RefundByReasonVm
            {
                Reason = reason,
                Count = refundRequests.Count(r => r.Reason == reason),
                Amount = refundRequests.Where(r => r.Reason == reason).Sum(r => r.Amount)
            }).ToList();

            var viewModel = new RefundsViewModel
            {
                TotalRefunds = totalRefunds,
                TotalRefundAmount = totalRefundAmount,
                ApprovedRefunds = approvedRefunds,
                PendingRefunds = pendingRefunds,
                RejectedRefunds = rejectedRefunds,
                AverageProcessingTime = averageProcessingTime,
                RefundRequests = refundRequests,
                RefundsByReason = refundsByReason,
                RefundPolicy = "Our standard refund policy allows customers to request refunds within 30 days of purchase. All refunds are subject to review and approval based on our terms and conditions."
            };

            return View(viewModel);
        }
    }
}