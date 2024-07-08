using AutoMapper;
using LibraNovel.Application.Exceptions;
using LibraNovel.Application.Interfaces;
using LibraNovel.Application.Parameters;
using LibraNovel.Application.ViewModels.Menu;
using LibraNovel.Application.ViewModels.Node;
using LibraNovel.Application.ViewModels.Novel;
using LibraNovel.Application.ViewModels.Permission;
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
    public class PermissionService : IPermissionService
    {
        private readonly LibraNovelContext _context;
        private readonly IMapper _mapper;

        public PermissionService(LibraNovelContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreatePermission(CreatePermissionViewModel request)
        {
            var permission = _mapper.Map<Permission>(request);

            await _context.Permissions.AddAsync(permission);
            await _context.SaveChangesAsync();
            return new Response<string>("Tạo thành công", null);
        }

        public async Task<Response<string>> DeletePermission(int permissionID)
        {
            var permission = await _context.Permissions.FindAsync(permissionID);
            if (permission == null)
            {
                throw new ApiException("Quyền không tồn tại hoặc đã bị xóa.");
            }

            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();
            return new Response<string>("Xóa thành công", null);
        }

        public async Task<Response<RequestParameter<PermissionResponse>>> GetAllPermissions(int pageIndex, int pageSize)
        {
            var permissions = await _context.Permissions.OrderBy(n => n.PermissionID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalCount = await _context.Permissions.CountAsync();

            var permissionsDTO = permissions.Select(n => _mapper.Map<PermissionResponse>(n)).ToList();

            if (permissionsDTO.Any())
            {
                foreach (var item in permissionsDTO)
                {
                    if (item.Parent != null)
                    {
                        var permission = await _context.Permissions.FindAsync(item.Parent);
                        item.ParentTitle = permission != null ? permission.Title : null;
                    }
                }
            }

            return new Response<RequestParameter<PermissionResponse>>
            {
                Succeeded = true,
                Data = new RequestParameter<PermissionResponse>
                {
                    TotalItemsCount = totalCount,
                    PageSize = pageSize,
                    PageIndex = pageIndex,
                    Items = permissionsDTO
                }
            };
        }

        public async Task<Response<List<NodeResponse>>> BuildTree()
        {
            var rootPermissions = new List<PermissionResponse>();
            var permissions = await _context.Permissions.ToListAsync();

            var permissionsDTO = permissions.Select(p => _mapper.Map<PermissionResponse>(p)).ToList();

            if (permissionsDTO.Any())
            {
                var permissionsDict = permissionsDTO.ToDictionary(m => m.PermissionID);

                foreach (var permission in permissionsDTO)
                {
                    if (permission.Parent == null)
                    {
                        rootPermissions.Add(permission);
                    }
                    else
                    {
                        if (permissionsDict.TryGetValue(permission.Parent.Value, out var parentPermission))
                        {
                            parentPermission.Children.Add(permission);
                        }
                    }
                }
            }

            return new Response<List<NodeResponse>>
            {
                Succeeded = true,
                Data = ConvertMenusToNodes(rootPermissions)
            };
        }

        private List<NodeResponse> ConvertMenusToNodes(List<PermissionResponse> permissions)
        {
            List<NodeResponse> nodes = new List<NodeResponse>();

            foreach (var permission in permissions)
            {
                var node = new NodeResponse
                {
                    Value = permission.PermissionID,
                    Label = permission.Title,
                    Children = ConvertMenusToNodes(permission.Children)
                };

                nodes.Add(node);
            }

            return nodes;
        }

        public async Task<Response<PermissionResponse>> GetPermissionByID(int permissionID)
        {
            var permission = await _context.Permissions.FirstOrDefaultAsync(n => n.PermissionID == permissionID);
            if (permission == null)
            {
                throw new ApiException("Quyền không tồn tại hoặc đã bị xóa.");
            }

            return new Response<PermissionResponse>(_mapper.Map<PermissionResponse>(permission), null);
        }

        public async Task<Response<string>> UpdatePermission(int permissionID, UpdatePermissionViewModel request)
        {
            if (permissionID != request.PermissionID)
            {
                throw new ApiException("Quyền không hợp lệ");
            }
            var permission = await _context.Permissions.FirstOrDefaultAsync(n => n.PermissionID == permissionID);
            if (permission == null)
            {
                throw new ApiException("Quyền không tồn tại hoặc đã bị xóa.");
            }
            _mapper.Map(request, permission);

            _context.Permissions.Update(permission);
            await _context.SaveChangesAsync();
            return new Response<string>("Cập nhật thành công", null);
        }

        public async Task<Response<List<PermissionResponse>>> GetPermissionsByRole(List<int> roleIDs)
        {
            List<Permission> permissionsByRole = new List<Permission>();
            var permissions = await _context.RolesPermissions.
                Join(_context.Roles, rp => rp.RoleID, r => r.RoleID, (rp, r) => new { r, rp }).
                Join(_context.Permissions, rp_p => rp_p.rp.PermissionID, p => p.PermissionID, (rp_p, p) => new { p, rp_p.rp }).
                ToListAsync();

            if (permissions.Any())
            {
                permissionsByRole = permissions.Where(result => roleIDs.Contains(result.rp.RoleID)).
                Select(result => result.p).DistinctBy(p => p.PermissionID).ToList();
            }

            return new Response<List<PermissionResponse>>
            {
                Succeeded = true,
                Data = permissionsByRole.Select(m => _mapper.Map<PermissionResponse>(m)).ToList()
            };
        }
    }
}
