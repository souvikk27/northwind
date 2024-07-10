using Microsoft.AspNetCore.Authorization;

namespace Northwind.Permissions
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string ControllerName { get; }
        public List<string> ActionNames { get; }

        public PermissionRequirement(string controllerName, List<string> actionNames)
        {
            ControllerName = controllerName;
            ActionNames = actionNames ?? new List<string>();
        }
    }
}
