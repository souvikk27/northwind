using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Permissions;
using System.Reflection;

namespace Northwind.Extensions;

public static class AuthorizationExtension
{
    public static void ConfigureAuthorizationPolicy(this IServiceCollection services)
    {
        GLobalAuthorizationPolicyRegistration(services);
        RoleResourceAuthPolicyRegistration(services);
    }

    private static void GLobalAuthorizationPolicyRegistration(IServiceCollection services)
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
        });
        services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
    }

    private static void RoleResourceAuthPolicyRegistration(IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("RoleResourcePolicy", policy =>
            {
                policy.RequireAssertion(context =>
                    context.Requirements.Any(_ =>
                        context.User.HasClaim(c =>
                            c is { Type: "Permission", Value: $"Roles-Manage" or $"Resources-Scopes" }
                        )
                    )
                );
            });
        });
        services.AddSingleton<IAuthorizationHandler, RoleResourceAuthRequirementHandler>();
    }
}