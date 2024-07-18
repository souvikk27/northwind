using Microsoft.AspNetCore.Authorization;

#nullable disable
namespace Northwind.Permissions
{
    public class RoleResourceAuthRequirement : IAuthorizationRequirement
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public RoleResourceAuthRequirement(string controllerName, string actionName)
        {
            ControllerName = controllerName;
            ActionName = actionName;
        }
    }
}