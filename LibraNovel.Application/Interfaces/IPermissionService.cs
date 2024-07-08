using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.Node;
using LibraNovel.Application.ViewModels.Permission;
using LibraNovel.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface IPermissionService
    {
        Task<Response<RequestParameter<PermissionResponse>>> GetAllPermissions(int pageIndex, int pageSize);
        Task<Response<List<NodeResponse>>> BuildTree();
        Task<Response<List<PermissionResponse>>> GetPermissionsByRole(List<int> roleIDs);
        Task<Response<PermissionResponse>> GetPermissionByID(int permissionID);
        Task<Response<string>> CreatePermission(CreatePermissionViewModel request);
        Task<Response<string>> UpdatePermission(int permissionID, UpdatePermissionViewModel request);
        Task<Response<string>> DeletePermission(int permissionID);
    }
}
