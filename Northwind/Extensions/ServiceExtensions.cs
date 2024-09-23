using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Northwind.Context;
using Northwind.Models;
using Northwind.Services.Railway.Resource;
using Northwind.Services.Railway.Roles;
using Northwind.Services.SQL;

namespace Northwind.Extensions;

public static class ServiceExtensions
{
	public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("Northwind")));
	}

	public static void UseAutoMigrations(this WebApplication app)
	{
		using (var scope = app.Services.CreateScope())
		{
			var services = scope.ServiceProvider;
			var context = services.GetRequiredService<ApplicationDbContext>();
			context.Database.Migrate();
		}
	}

	public static void ConfigureSqlConnectionFactory(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("Northwind") ??
							   throw new ArgumentNullException(nameof(configuration));
		services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
	}

	public static void ConfigureIdentity(this IServiceCollection services)
	{
		services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();
	}

	public static void ConfigureRailwayPattern(this IServiceCollection services)
	{
		services.AddScoped<IRoleService, RoleService>();
		services.AddScoped<IResourceService, ResourceService>();
	}

	public static void ConfigureMediatR(this IServiceCollection services)
	{
		services.AddMediatR(configuration =>
				configuration.RegisterServicesFromAssembly(
					typeof(ServiceExtensions).Assembly
					));
	}
}