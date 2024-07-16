
using LibraNovel.Application.ViewModels.Dashboard;
using LibraNovel.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<Response<DashboardReponse>> GetDashboardData(Guid userID);
    }
}
