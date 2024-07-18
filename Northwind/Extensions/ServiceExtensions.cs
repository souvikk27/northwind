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

            foreach (var policyName in from controller in controllers
                     let controllerName = controller.Name.Replace("Controller", "")
                     let actions = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                         .Where(m =>
                             (typeof(IActionResult).IsAssignableFrom(m.ReturnType) ||
                              typeof(Task<IActionResult>).IsAssignableFrom(m.ReturnType)) &&
                             m.DeclaringType == controller)
                         .Select(m => m.Name)
                         .ToList()
                     from action in actions
                     let actionName = action
                     select $"{controllerName}-{actionName}")
            {
                options.AddPolicy(policyName, policy =>
                    policy.RequireClaim("Permission", policyName));
            }

            options.AddPolicy("RoleResourcePolicy", policy =>
            {
                policy.RequireAssertion(context =>
                    context.Requirements.Any(r =>
                        context.User.HasClaim(c =>
                            c is { Type: "Permission", Value: $"Roles-Manage" or $"Resources-Scopes" }
                        )
                    )
                );
            });
        });

        services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
        services.AddSingleton<IAuthorizationHandler, RoleResourceAuthRequirementHandler>();
    }
}