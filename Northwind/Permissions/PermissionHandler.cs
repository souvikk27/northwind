using Microsoft.AspNetCore.Authorization;

namespace Northwind.Permissions
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var userClaims = context.User.Claims;

            // Check if the user has a permission claim for the controller and any of the actions
            if (requirement.ActionNames.Any(action => userClaims.Any(c => c.Type == "Permission" &&
                                                                          c.Value ==
                                                                          $"{requirement.ControllerName}.{action}")))
            {
                context.Succeed(requirement);
            }

            await Task.Yield();
        }
    }
}