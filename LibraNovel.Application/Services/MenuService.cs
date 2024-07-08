using AutoMapper;
using LibraNovel.Application.Exceptions;
using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.Menu;
using LibraNovel.Application.ViewModels.Node;
using LibraNovel.Application.ViewModels.Permission;
using LibraNovel.Application.ViewModels.Role;
using LibraNovel.Application.Wrappers;
using LibraNovel.Domain.Models;
using LibraNovel.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly LibraNovelContext _context;
        private readonly IMapper _mapper;

        public MenuService(LibraNovelContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<List<NodeResponse>>> BuildTree()
        {
            var menus = await _context.Menus.ToListAsync();

            var menusDTO = menus.Select(m => _mapper.Map<MenuResponse>(m)).ToList();

            return new Response<List<NodeResponse>>
            {
                Succeeded = true,
                Data = ConvertMenusToNodes(menusDTO)
            };
        }

        private List<NodeResponse> ConvertMenusToNodes(List<MenuResponse> menus)
        {
            List<NodeResponse> nodes = new List<NodeResponse>();

            foreach (var menu in menus)
            {
                var node = new NodeResponse
                {
                    Value = menu.MenuID,
                    Label = menu.Title,
                };

                nodes.Add(node);
            }

            return nodes;
        }

        public async Task<Response<string>> CreateMappingRoleWithMenus(CreateMappingRoleWithMenuViewModel request)
        {
            //Role menus = rms
            var rms = await _context.RolesMenus.Where(rm => rm.RoleID == request.RoleID).Select(rp => rp.MenuID).ToListAsync();

            List<int> newMenus;
            try
            {
                newMenus = request.Menus.Select(int.Parse).ToList();
            }
            catch (FormatException)
            {
                throw new ApiException("Một trong số các menu không phải định dạng số nguyên.");
            }

            foreach (var menuId in GetAddList(rms, newMenus))
            {
                await _context.RolesMenus.AddAsync(new RolesMenu { RoleID = request.RoleID, MenuID = menuId });
            }

            foreach (var menuId in GetRemoveList(rms, newMenus))
            {
                var roleMenus = await _context.RolesMenus
                                                   .FirstOrDefaultAsync(rm => rm.RoleID == request.RoleID && rm.MenuID == menuId);
                if (roleMenus != null)
                {
                    _context.RolesMenus.Remove(roleMenus);
                }
            }

            await _context.SaveChangesAsync();

            return new Response<string>("Liên kết menu và vai trò thành công", null);
        }

        private List<int> GetAddList(List<int> oldMenus, List<int> newMenus)
        {
            List<int> toAdd = new List<int>();
            foreach (var menu in newMenus)
            {
                if (!oldMenus.Contains(menu))
                {
                    toAdd.Add(menu);
                }
            }
            return toAdd;
        }

        private List<int> GetRemoveList(List<int> oldMenus, List<int> newMenus)
        {
            List<int> toRemove = new List<int>();
            foreach (var menu in oldMenus)
            {
                if (!newMenus.Contains((int)menu!))
                {
                    toRemove.Add((int)menu);
                }
            }
            return toRemove;
        }

        public async Task<Response<string>> CreateMenu(CreateMenuViewModel request)
        {
            var menu = _mapper.Map<Menu>(request);
            await _context.Menus.AddAsync(menu);
            await _context.SaveChangesAsync();
            return new Response<string>("Tạo menu thành công", null);
        }

        public async Task<Response<string>> DeleteMenu(int menuID)
        {
            var menu = await _context.Menus.FindAsync(menuID);
            if(menu == null)
            {
                throw new ApiException("Menu không tồn tại");
            }

            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();
            return new Response<string>("Xóa menu thành công", null);
        }

        public async Task<Response<List<MenuResponse>>> GetAllMenus()
        {
            var menus = await _context.Menus.ToListAsync();

            return new Response<List<MenuResponse>>
            {
                Succeeded = true,
                Data = menus.Select(m => _mapper.Map<MenuResponse>(m)).ToList()
            };
        }

        public async Task<Response<MenuResponse>> GetMenuByID(int menuID)
        {
            var menu = await _context.Menus.FindAsync(menuID);
            if(menu == null)
            {
                throw new ApiException("Menu không tồn tại");
            }
            return new Response<MenuResponse> (_mapper.Map<MenuResponse>(menu), null);    
        }

        public async Task<Response<List<MenuResponse>>> GetMenusByRole(List<int> roleIDs)
        {
            List<Menu> menusByRole = new List<Menu>();
            var menus = await _context.RolesMenus.
                Join(_context.Roles, rm => rm.RoleID, r => r.RoleID, (rm, r) => new { r, rm }).
                Join(_context.Menus, rm_m => rm_m.rm.MenuID, m => m.MenuID, (rm_m, m) => new { m, rm_m.rm }).
                ToListAsync();

            if(menus.Any())
            {
                menusByRole = menus.Where(result => roleIDs.Contains(result.rm.RoleID)).
                Select(result => result.m).DistinctBy(m => m.MenuID).ToList();
            }

            return new Response<List<MenuResponse>>
            {
                Succeeded = true,
                Data = menusByRole.Select(m => _mapper.Map<MenuResponse>(m)).ToList()
            };
        }

        public async Task<Response<string>> UpdateMenu(int menuID, UpdateMenuViewModel request)
        {
            if(menuID != request.MenuID)
            {
                throw new ApiException("Menu không hợp lệ");
            }

            var menu = await _context.Menus.FindAsync(menuID);
            if(menu == null)
            {
                throw new ApiException("Menu không tồn tại");
            }

            _mapper.Map(request, menu);
            _context.Menus.Update(menu);
            await _context.SaveChangesAsync();
            return new Response<string>("Cập nhật thành công", null);
        }
    }
}
