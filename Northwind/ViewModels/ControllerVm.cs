#nullable disable

namespace Northwind.ViewModels;

public class ControllerVm
{
    public string RoleId { get; set; }
    public string Name { get; set; }

    public List<ActionInfoVm> ActionMethods { get; set; }
}