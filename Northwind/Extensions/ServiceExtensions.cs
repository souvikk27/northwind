using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Northwind.Context;
using Northwind.Models;
using Northwind.Permissions;
using Northwind.Services.Railway.Resource;
using Northwind.Services.Railway.Roles;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

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
        services.AddAuthorization(options =>
        {
            // Get all controller types in the assembly
            var controllers = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                .ToList();

            foreach (var controller in controllers)
            {
                var controllerName = controller.Name.Replace("Controller", "");

                // Get all public action methods of the controller
                var actions = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m =>
                        (typeof(IActionResult).IsAssignableFrom(m.ReturnType) ||
                         typeof(Task<IActionResult>).IsAssignableFrom(m.ReturnType)) &&
                        m.DeclaringType == controller)
                    .Select(m => m.Name)
                    .ToList();

                //Iterate through action name and set according to criteria
                foreach (var action in actions)
                {
                    var actionName = action;
                    var policyName = $"{controllerName}-{actionName}";

                    options.AddPolicy(policyName, policy =>
                        policy.RequireClaim("Permission", policyName));
                }
            }
        });

        services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
    }
}