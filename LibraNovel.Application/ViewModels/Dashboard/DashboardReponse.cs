using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Dashboard
{
    public class DashboardReponse
    {
        public NovelDashboard NovelDashboard {  get; set; }
        public UserCreatedDashboard UserCreatedDashboard { get; set; }
        public CommentDashboard CommentDashboard { get; set; }
        public OperatingTimeDashboard OperatingTimeDashboard { get; set; }
    }

    public class NovelDashboard
    {
        public int Total { get; set; }
        public double Percentage { get; set; }
    }

    public class UserCreatedDashboard
    {
        public int Total { get; set; }
        public double Percentage { get; set; }
    }

    public class CommentDashboard
    {
        public int Total { get; set; }
        public double Percentage { get; set; }
    }

    public class OperatingTimeDashboard
    {
        public double Hours { get; set; }
        public DateTime LastLoginTime { get; set; }
    }
}
