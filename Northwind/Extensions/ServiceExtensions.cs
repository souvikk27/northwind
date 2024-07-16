using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Northwind.Context;
using Northwind.Models;
using Northwind.Services.Railway;
using Northwind.Services.Railway.Resource;
using Northwind.Services.Railway.Roles;

namespace Northwind.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Northwind")));
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
    
    
    public static void ConfigureAuthorizationPolicy(this IServiceCollection services)
    {
	    
	}
}