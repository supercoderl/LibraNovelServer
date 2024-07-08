using AutoMapper;
using CloudinaryDotNet.Actions;
using LibraNovel.Application.Exceptions;
using LibraNovel.Application.Interfaces;
using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.Genre;
using LibraNovel.Application.ViewModels.Role;
using LibraNovel.Application.ViewModels.UsersRoles;
using LibraNovel.Application.Wrappers;
using LibraNovel.Domain.Models;
using LibraNovel.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Role = LibraNovel.Domain.Models.Role;

namespace LibraNovel.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly LibraNovelContext _context;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;
        private readonly IMenuService _menuService;

        public RoleService(LibraNovelContext context, IMapper mapper, IPermissionService permissionService, IMenuService menuService)
        {
            _context = context;
            _mapper = mapper;
            _permissionService = permissionService;
            _menuService = menuService;
        }

        public async Task<Response<bool>> CheckAnyUserHaveRole()
        {
            var check = await _context.UsersRoles.AnyAsync(ur => ur.UserID != null);
            return new Response<bool>(check, null);
        }

        public async Task<Response<string>> CreateMappingRoleWithPermissions(CreateMappingRoleWithPermissionsRequest request)
        {
            //Role permissions = rps
            var rps = await _context.RolesPermissions.Where(rp => rp.RoleID == request.RoleID).Select(rp => rp.PermissionID).ToListAsync();

            List<int> newPermissions;
            try
            {
                newPermissions = request.Permissions.Select(int.Parse).ToList();
            }
            catch (FormatException)
            {
                throw new ApiException("Một trong số các quyền không phải định dạng số nguyên.");
            }

            foreach (var permissionId in GetAddList(rps, newPermissions))
            {
                await _context.RolesPermissions.AddAsync(new RolesPermission { RoleID = request.RoleID, PermissionID = permissionId });
            }

            foreach (var permissionId in GetRemoveList(rps, newPermissions))
            {
                var rolePermission = await _context.RolesPermissions
                                                   .FirstOrDefaultAsync(rp => rp.RoleID == request.RoleID && rp.PermissionID == permissionId);
                if (rolePermission != null)
                {
                    _context.RolesPermissions.Remove(rolePermission);
                }
            }

            await _context.SaveChangesAsync();

            return new Response<string>("Liên kết quyền và vai trò thành công", null);
        }

        private List<int> GetAddList(List<int> oldPermissions, List<int> newPermissions)
        {
            List<int> toAdd = new List<int>();
            foreach (var permission in newPermissions)
            {
                if (!oldPermissions.Contains(permission))
                {
                    toAdd.Add(permission);
                }
            }
            return toAdd;
        }

        private List<int> GetRemoveList(List<int> oldPermissions, List<int> newPermissions)
        {
            List<int> toRemove = new List<int>();
            foreach (var permission in oldPermissions)
            {
                if (!newPermissions.Contains((int)permission!))
                {
                    toRemove.Add((int)permission);
                }
            }
            return toRemove;
        }

        public async Task<Response<string>> CreateRole(CreateRoleViewModel request)
        {
            var role = _mapper.Map<Role>(request);
            await _context.AddAsync(role);
            await _context.SaveChangesAsync();
            return new Response<string>("Tạo vai trò thành công", null);
        }

        public async Task<Response<string>> DeleteRole(int roleID)
        {
            var role = await _context.Roles.FindAsync(roleID);
            if (role == null)
            {
                throw new ApiException("Vai trò không tồn tại.");
            }
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return new Response<string>("Xóa vai trò thành công", null);
        }

        public async Task<Response<RequestParameter<RoleResponse>>> GetAllRoles(int pageIndex, int pageSize)
        {
            var roles = await _context.Roles.OrderBy(r => r.RoleID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            var rolesDTO = roles.Select(r => _mapper.Map<RoleResponse>(r)).ToList();

            if(rolesDTO.Any())
            {
                foreach(var role in rolesDTO)
                {
                    var pResult = await _permissionService.GetPermissionsByRole([role.RoleID]);
                    if(pResult.Succeeded)
                    {
                        role.Permissions = pResult.Data.Select(p => p.PermissionID.ToString()).ToList();
                    }

                    var mResult = await _menuService.GetMenusByRole([role.RoleID]);
                    if (mResult.Succeeded)
                    {
                        role.Menus = mResult.Data.Select(m => m.MenuID.ToString()).ToList();
                    }
                }
            }


            return new Response<RequestParameter<RoleResponse>>
            {
                Succeeded = true,
                Data = new RequestParameter<RoleResponse>
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalItemsCount = roles.Count,
                    Items = rolesDTO,
                }
            };
        }

        public async Task<Response<IReadOnlyList<RoleResponse>>> GetMappingRoles(Guid userID)
        {
            var roles = await _context.UsersRoles.Where(ur => ur.UserID == userID).
                Join(_context.Roles, ur => ur.RoleID, r => r.RoleID, (ur, r) => new { r, ur }).
                Join(_context.Users, combine => combine.ur.UserID, u => u.UserID, (combine, u) => combine.r).ToListAsync();
            return new Response<IReadOnlyList<RoleResponse>>
            {
                Succeeded = true,
                Data = roles.Select(r => _mapper.Map<RoleResponse>(r)).ToList()
            };
        }

        public async Task<Response<RoleResponse>> GetRoleByID(int roleID)
        {
            var role = await _context.Roles.FindAsync(roleID);
            if (role == null)
            {
                throw new ApiException("Vai trò không tồn tại");
            }
            return new Response<RoleResponse>
            {
                Succeeded = true,
                Data = _mapper.Map<RoleResponse>(role)
            };
        }

        public async Task<Response<string>> UpdateRole(int roleID, UpdateRoleViewModel request)
        {
            if (roleID != request.RoleID)
            {
                throw new ApiException("Vai trò không hợp lệ");
            }

            var role = await _context.Roles.FindAsync(roleID);
            if (role == null)
            {
                throw new ApiException("Vai trò không tồn tại");
            }

            _mapper.Map(request, role);
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return new Response<string>("Cập nhật thành công", null);
        }
    }
}
