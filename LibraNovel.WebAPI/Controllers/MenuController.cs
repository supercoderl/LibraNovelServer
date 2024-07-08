using LibraNovel.Application.Interfaces;
using LibraNovel.Application.Services;
using LibraNovel.Application.ViewModels.Menu;
using LibraNovel.Application.ViewModels.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraNovel.WebAPI.Controllers
{
    [Authorize]
    public class MenuController : BaseApiController
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet("/get-all-menus")]
        public async Task<IActionResult> GetAllMenus()
        {
            return Ok(await _menuService.GetAllMenus());
        }

        [HttpGet("/get-menus-by-role")]
        public async Task<IActionResult> GetMenusByRole()
        {
            var roles = User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();
            return Ok(await _menuService.GetMenusByRole(roles.Select(r => int.Parse(r)).ToList()));
        }

        [HttpGet("/get-menu-by-id/{menuID}")]
        public async Task<IActionResult> GetMenuByID(int menuID)
        {
            return Ok(await _menuService.GetMenuByID(menuID));
        }

        [HttpGet("/get-menus-tree")]
        public async Task<IActionResult> GetMenusTree()
        {
            return Ok(await _menuService.BuildTree());
        }

        [HttpPost("/create-mapping-role-with-menus")]
        public async Task<IActionResult> CreateMappingRoleWithMenus(CreateMappingRoleWithMenuViewModel request)
        {
            return Ok(await _menuService.CreateMappingRoleWithMenus(request));
        }

        [HttpPost("/create-menu")]
        public async Task<IActionResult> CreateMenu(CreateMenuViewModel request)
        {
            return Ok(await _menuService.CreateMenu(request));
        }

        [HttpPut("/update-menu/{menuID}")]
        public async Task<IActionResult> UpdateMenu(int menuID, UpdateMenuViewModel request)
        {
            return Ok(await _menuService.UpdateMenu(menuID, request));
        }

        [HttpDelete("/delete-menu/{menuID}")]
        public async Task<IActionResult> DeleteMenu(int menuID)
        {
            return Ok(await _menuService.DeleteMenu(menuID));
        }
    }
}
