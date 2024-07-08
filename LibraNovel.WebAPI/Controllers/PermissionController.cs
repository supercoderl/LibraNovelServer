using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.Novel;
using LibraNovel.Application.ViewModels.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraNovel.WebAPI.Controllers
{
    public class PermissionController : BaseApiController
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet("/get-all-permissions")]
        public async Task<IActionResult> GetPermissions(int pageIndex = 1, int pageSize = 10)
        {
            return Ok(await _permissionService.GetAllPermissions(pageIndex, pageSize));
        }

        [HttpGet("/get-permissions-tree")]
        public async Task<IActionResult> GetPermissionsTree()
        {
            return Ok(await _permissionService.BuildTree());
        }

        [HttpGet("/get-permissions-by-role")]
        [Authorize]
        public async Task<IActionResult> GetPermissionsByRole()
        {
            var roles = User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => int.Parse(c.Value))
            .ToList();
            return Ok(await _permissionService.GetPermissionsByRole(roles));
        }

        [HttpGet("/get-permission-by-id/{permissionID}")]
        public async Task<IActionResult> GetPermissionByID(int permissionID)
        {
            return Ok(await _permissionService.GetPermissionByID(permissionID));
        }

        [HttpPost("/create-permission")]
        [Authorize]
        public async Task<IActionResult> CreatePermission(CreatePermissionViewModel request)
        {
            var userID = User.FindFirstValue("UserID");
            if (userID == null)
            {
                return Unauthorized();
            }

            request.CreatedBy = Guid.Parse(userID);

            return Ok(await _permissionService.CreatePermission(request));
        }

        [HttpPut("/update-permission/{permissionID}")]
        [Authorize]
        public async Task<IActionResult> UpdatePermission(int permissionID, UpdatePermissionViewModel request)
        {
            var userID = User.FindFirstValue("UserID");
            if (userID == null)
            {
                return Unauthorized();
            }

            request.UpdatedBy = Guid.Parse(userID);
            return Ok(await _permissionService.UpdatePermission(permissionID, request));
        }

        [HttpDelete("/delete-permission/{permissionID}")]
        [Authorize]
        public async Task<IActionResult> DeletePermission(int permissionID)
        {
            return Ok(await _permissionService.DeletePermission(permissionID));
        }
    }
}
