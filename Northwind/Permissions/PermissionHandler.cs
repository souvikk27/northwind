using Microsoft.AspNetCore.Authorization;

namespace Northwind.Permissions
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var userClaims = context.User.Claims;

            // Check if the user has a permission claim for the controller and any of the actions
            if (requirement.ActionNames.Any(action => userClaims.Any(c => c.Type == "Permission" &&
                                                                          c.Value == $"{requirement.ControllerName}.{action}")))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}