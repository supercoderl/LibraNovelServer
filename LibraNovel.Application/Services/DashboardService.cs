using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.Dashboard;
using LibraNovel.Application.Wrappers;
using LibraNovel.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly LibraNovelContext _context;

        public DashboardService(LibraNovelContext context)
        {
            _context = context;
        }

        public async Task<Response<DashboardReponse>> GetDashboardData(Guid userID)
        {
            var currentDate = DateTime.UtcNow;
            var previousMonthDate = currentDate.AddMonths(-1);

            return new Response<DashboardReponse>
            {
                Succeeded = true,
                Data = new DashboardReponse
                {
                    NovelDashboard = await GetNovelData(currentDate, previousMonthDate),
                    UserCreatedDashboard = await GetUserData(currentDate, previousMonthDate),
                    CommentDashboard = await GetCommentData(currentDate, previousMonthDate),
                    OperatingTimeDashboard = await CalculateUserActivityTime(currentDate, userID)
                }
            };
        }

        private async Task<NovelDashboard> GetNovelData(DateTime currentDate, DateTime previousDate)
        {
            var totalCountThisMonth = await _context.Novels.AsNoTracking()
                                           .Where(n => n.PublishedDate.Month == currentDate.Month && n.PublishedDate.Year == currentDate.Year)
                                           .CountAsync();
            
            var totalCountPreviousMonth = await _context.Novels.AsNoTracking()
                               .Where(n => n.PublishedDate.Month == previousDate.Month && n.PublishedDate.Year == previousDate.Year)
                               .CountAsync();

            var percentage = CalculatePercentageChange(totalCountThisMonth, totalCountPreviousMonth);

            return new NovelDashboard
            {
                Total = await _context.Novels.AsNoTracking().CountAsync(),
                Percentage = percentage
            };
        }

        private async Task<UserCreatedDashboard> GetUserData(DateTime currentDate, DateTime previousDate)
        {
            var totalCountThisMonth = await _context.Users.AsNoTracking()
                                           .Where(n => n.RegistrationDate.Month == currentDate.Month && n.RegistrationDate.Year == currentDate.Year)
                                           .CountAsync();

            var totalCountPreviousMonth = await _context.Users.AsNoTracking()
                               .Where(n => n.RegistrationDate.Month == previousDate.Month && n.RegistrationDate.Year == previousDate.Year)
                               .CountAsync();

            var percentage = CalculatePercentageChange(totalCountThisMonth, totalCountPreviousMonth);

            return new UserCreatedDashboard
            {
                Total = totalCountThisMonth,
                Percentage = percentage
            };
        }

        private async Task<CommentDashboard> GetCommentData(DateTime currentDate, DateTime previousDate)
        {
            var totalCountThisMonth = await _context.Comments.AsNoTracking()
                                           .Where(n => n.CreatedDate.Month == currentDate.Month && n.CreatedDate.Year == currentDate.Year)
                                           .CountAsync();

            var totalCountPreviousMonth = await _context.Comments.AsNoTracking()
                               .Where(n => n.CreatedDate.Month == previousDate.Month && n.CreatedDate.Year == previousDate.Year)
                               .CountAsync();

            var percentage = CalculatePercentageChange(totalCountThisMonth, totalCountPreviousMonth);

            return new CommentDashboard
            {
                Total = totalCountThisMonth,
                Percentage = percentage
            };
        }

        private double CalculatePercentageChange(int current, int previous)
        {
            if (previous == 0) return 100;
            return ((double)(current - previous) / previous) * 100;
        }

        private async Task<OperatingTimeDashboard> CalculateUserActivityTime(DateTime currentDate, Guid userID)
        {
            var query = _context.Users.AsNoTracking().Where(u => u.UserID == userID);
/*            var userActivityLogs = await query.Where(log => log.LastLoggedIn.Month == currentDate.Month && log.LastLoggedIn.Year == currentDate.Year)
                .AverageAsync(u => (currentDate - u.LastLoggedIn).TotalHours);*/

            return new OperatingTimeDashboard
            {
                Hours = 20.5,
                LastLoginTime = await query.Select(u => u.LastLoggedIn).FirstOrDefaultAsync()
            };
        }
    }
}
