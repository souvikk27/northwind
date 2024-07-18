#nullable disable
using Microsoft.AspNetCore.Authorization;

namespace Northwind.Permissions;

public class RoleResourceAuthRequirementHandler : AuthorizationHandler<RoleResourceAuthRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        RoleResourceAuthRequirement requirement)
    {
        var userClaims = context.User.Claims;
        if (userClaims.Any(c =>
                c.Type == "Permission" && c.Value == $"{requirement.ControllerName}-{requirement.ActionName}"))
        {
            context.Succeed(requirement);
        }

        await Task.Yield();
    }
}