using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraNovel.WebAPI.Controllers
{
    public class RoleController : BaseApiController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("/get-all-roles")]
        public async Task<IActionResult> GetRoles(int pageIndex = 1, int pageSize = 10)
        {
            return Ok(await _roleService.GetAllRoles(pageIndex, pageSize));
        }

        [HttpGet("/get-role-by-id/{roleID}")]
        public async Task<IActionResult> GetRoleByID(int roleID)
        {
            return Ok(await _roleService.GetRoleByID(roleID));
        }

        [HttpPost("/create-mapping-role-with-permissions")]
        public async Task<IActionResult> CreateMappingRoleWithPermissions(CreateMappingRoleWithPermissionsRequest request)
        {
            return Ok(await _roleService.CreateMappingRoleWithPermissions(request));
        }

        [HttpPost("/create-role")]
        public async Task<IActionResult> CreateNewRole(CreateRoleViewModel request)
        {
            return Ok(await _roleService.CreateRole(request));
        }

        [HttpPut("/update-role/{roleID}")]
        public async Task<IActionResult> UpdateRole(int roleID, UpdateRoleViewModel request)
        {
            return Ok(await _roleService.UpdateRole(roleID, request));  
        }

        [HttpDelete("/delete-role/{roleID}")]
        public async Task<IActionResult> DeleteRole(int roleID)
        {
            return Ok(await _roleService.DeleteRole(roleID)); ;
        }
    }
}
