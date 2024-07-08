using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.Chapter;
using LibraNovel.Application.ViewModels.Role;
using LibraNovel.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface IRoleService
    {
        Task<Response<RequestParameter<RoleResponse>>> GetAllRoles(int pageIndex, int pageSize);
        Task<Response<IReadOnlyList<RoleResponse>>> GetMappingRoles(Guid userID);
        Task<Response<RoleResponse>> GetRoleByID(int roleID);
        Task<Response<string>> CreateMappingRoleWithPermissions(CreateMappingRoleWithPermissionsRequest request);
        Task<Response<bool>> CheckAnyUserHaveRole();
        Task<Response<string>> CreateRole(CreateRoleViewModel request);
        Task<Response<string>> UpdateRole(int roleID, UpdateRoleViewModel request);
        Task<Response<string>> DeleteRole(int roleID);
    }
}
