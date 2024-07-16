using Northwind.ViewModels;

namespace Northwind.Services.Railway.Resource
{
    public interface IResourceService
    {
        Task<Result<List<ControllerVm>>> GetScopesResult(string id);
        Task<Result<bool>> ValidateAndProcessPermissions(List<RolePermissionUpdateVm> permissions);
    }
}
