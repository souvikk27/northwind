namespace Northwind.ViewModels;
#nullable disable
public class RolePermissionUpdateVm
{
    public string RoleId { get; set; }
    public string ControllerName { get; set; }
    public List<ActionPermission> Actions { get; set; }
}

public class ActionPermission
{
    public string ActionMethodName { get; set; }
    public bool IsAllowed { get; set; }
}