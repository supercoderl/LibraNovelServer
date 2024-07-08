using LibraNovel.Application.ViewModels.Menu;
using LibraNovel.Application.ViewModels.Node;
using LibraNovel.Application.ViewModels.Role;
using LibraNovel.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface IMenuService
    {
        Task<Response<List<MenuResponse>>> GetAllMenus();
        Task<Response<MenuResponse>> GetMenuByID(int menuID);
        Task<Response<List<MenuResponse>>> GetMenusByRole(List<int> roleID);
        Task<Response<string>> CreateMenu(CreateMenuViewModel request);
        Task<Response<List<NodeResponse>>> BuildTree();
        Task<Response<string>> CreateMappingRoleWithMenus(CreateMappingRoleWithMenuViewModel request);
        Task<Response<string>> UpdateMenu(int menuID, UpdateMenuViewModel request);
        Task<Response<string>> DeleteMenu(int menuID);  
    }
}
