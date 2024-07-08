using Microsoft.EntityFrameworkCore;
using Northwind.Context;

namespace Northwind.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Northwind")));
    }
}